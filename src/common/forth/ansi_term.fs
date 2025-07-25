\ Copyright (c) 2021-2025 Travis Bemann
\ Copyright (c) 2025 tmsgthb (GitHub)
\ 
\ Permission is hereby granted, free of charge, to any person obtaining a copy
\ of this software and associated documentation files (the "Software"), to deal
\ in the Software without restriction, including without limitation the rights
\ to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
\ copies of the Software, and to permit persons to whom the Software is
\ furnished to do so, subject to the following conditions:
\ 
\ The above copyright notice and this permission notice shall be included in
\ all copies or substantial portions of the Software.
\ 
\ THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
\ IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
\ FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
\ AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
\ LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
\ OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
\ SOFTWARE.

\ Compile this to flash
compile-to-flash

begin-module ansi-term

  systick import

  begin-module ansi-term-internal

    \ Saved entered byte
    user saved-key

    \ Show cursor count
    user show-cursor-count

    \ Preserve cursor count
    user preserve-cursor-count

    \ Background color offset
    10 constant background-offset
    
  end-module> import
  
  compress-flash

  \ Character constants
  $1B constant escape

  \ No color or font set
  00 constant none

  \ Font constants
  01 constant bold
  02 constant dim
  22 constant normal
  04 constant underline
  24 constant underline-off
  07 constant reverse-color
  27 constant reverse-color-off

  \ Color constants
  30 constant black
  31 constant red
  32 constant green
  33 constant yellow
  34 constant blue
  35 constant magenta
  36 constant cyan
  37 constant white
  90 constant b-black
  91 constant b-red
  92 constant b-green
  93 constant b-yellow
  94 constant b-blue
  95 constant b-magenta
  96 constant b-cyan
  97 constant b-white
  39 constant default-color
  
  \ Not a color exception
  : x-not-color ( -- ) ." not a color" cr ;

  \ Create a background color
  : background { color -- color' }
    color black >= color white <= and
    color b-black >= color b-white <= and or
    color default-color = or if
      color background-offset +
    else
      color [ black background-offset + ] literal >=
      color [ white background-offset + ] literal <= and
      color [ b-black background-offset + ] literal >=
      color [ b-white background-offset + ] literal <= and or
      color [ default-color background-offset + ] literal = or
      averts x-not-color
      color
    then
  ;
  
  \ Type a decimal integer
  : (dec.) ( n -- ) base @ 10 base ! swap (.) base ! ;

  \ Type the CSI sequence
  : csi ( -- )
     $1B emit [char] [ emit
  ;

  \ End of color and font efectType the end of CSI sequence
  : end-color-effect ( -- )
    [char] m emit 
  ;

  commit-flash

  \ Show the cursor
  : show-cursor ( -- )
    csi [char] ? emit 25 (dec.) [char] h emit
  ;

  \ Hide the cursor
  : hide-cursor ( -- )
    csi [char] ? emit 25 (dec.) [char] l emit
  ;

  \ Save the cursor position
  : save-cursor ( -- ) csi [char] s emit ;

  \ Restore the cursor position
  : restore-cursor ( -- ) csi [char] u emit ;

  \ Scroll up screen by a number of lines
  : scroll-up ( lines -- )
    csi (dec.) [char] S emit
  ;

  \ Move the cursor to specified row and column coordinates
  : go-to-coord ( row column -- )
    swap csi 1+ (dec.) [char] ; emit 1+ (dec.) [char] f emit
  ;

  \ Erase from the cursor to the end of the line
  : erase-end-of-line ( -- ) csi [char] K emit ;

  \ Erase the lines below the current line
  : erase-down ( -- ) csi [char] J emit ;

  \ Query for the cursor position
  : query-cursor-position ( -- )
    csi [char] 6 emit [char] n emit
  ;

  commit-flash
  
  \ Reset the terminal cursor
  : reset-terminal-cursor ( -- )
    0 show-cursor-count ! show-cursor
  ;

  \ Reset the terminal color
  : reset-terminal-color ( -- )
    csi 0 (dec.) [char] m emit
  ;

  \ Show the cursor with a show/hide counter
  : show-cursor ( -- )
    1 show-cursor-count +! show-cursor-count @ 0 = if show-cursor then
  ;

  \ Hide the cursor with a show/hide counter
  : hide-cursor ( -- )
    -1 show-cursor-count +! show-cursor-count @ -1 = if hide-cursor then
  ;

  commit-flash

  \ Execute code with a hidden cursor
  : execute-hide-cursor ( xt -- ) hide-cursor try show-cursor ?raise ;

  \ Clear a saved key
  : clear-key ( -- ) 0 saved-key ! ;

  \ Get a key
  : get-key ( -- b )
    saved-key @ ?dup if 0 saved-key ! else key then
  ;

  \ Get whether a key is available
  : get-key? ( -- flag ) saved-key @ if true else key? then ;

  \ Save a key
  : set-key ( b -- ) saved-key ! ;

  commit-flash
  
  \ Wait for a number
  : wait-number ( -- n matches )
    ram-here >r get-key dup [char] - = if
      ram-here c! 1 ram-allot
    else
      set-key
    then
    0 begin
      dup 10 < if
	get-key dup [char] 0 >= over [char] 9 <= and if
	  ram-here c! 1 ram-allot 1+ false
	else
	  set-key true
	then
      else
	true
      then
    until
    drop base @ 10 base ! r@ ram-here r@ - parse-unsigned
    rot base ! r> ram-here!
  ;
  
  \ Wait for a character
  : wait-char ( b -- ) begin dup get-key = until drop ;

  \ Confirm that a character is what is expected
  : expect-char ( b -- flag )
    get-key dup rot = if drop true else set-key false then
  ;

  commit-flash
  
  \ Get the cursor position
  : get-cursor-position ( -- row column )
    [:
      begin
	clear-key query-cursor-position escape wait-char
	[char] [ expect-char if
	  wait-number if
	    1 - [char] ; expect-char if
	      wait-number if
		[char] R expect-char if
		  1 - true
		else
		  2drop false
		then
	      else
		drop false
	      then
	    else
	      drop false
	    then
	  else
	    drop false
	  then
	else
	  false
	then
      until
    ;] execute-hide-cursor
  ;

  commit-flash

  \ Execute code while preserving cursor position
  : execute-preserve-cursor ( xt -- )
    1 preserve-cursor-count +!
    preserve-cursor-count @ 1 = if
      save-cursor try restore-cursor
    else
      get-cursor-position >r >r try r> r> go-to-coord
    then
    -1 preserve-cursor-count +! ?raise
  ;

  \ Actually get the terminal size
  : get-terminal-size ( -- rows columns )
    [:
      get-cursor-position
      1000 1000 go-to-coord
      get-cursor-position swap 1+ swap 1+
      2swap go-to-coord
    ;] execute-hide-cursor
  ;

  \ Reset terminal state
  : reset-ansi-term ( -- )
    0 show-cursor-count ! 0 preserve-cursor-count ! 0 saved-key !
  ;
  
  \ Clear window in ticks
  100 constant clear-ticks

  commit-flash
  
  \ Clear input of multiple escaped characters
  : clear-keys ( -- )
    systick-counter clear-ticks +
    begin
      get-key? if
	get-key dup escape = if
	  set-key true
	else
	  drop dup systick-counter <
        then
      else
        pause dup systick-counter <
      then
    until
    drop
  ;
 
  \ Set font effect or color
  : color-effect! ( color-effect -- ) csi (dec.) end-color-effect ;
 
end-module

commit-flash

\ Clear the console
: page ( -- )
  ansi-term::csi [char] 2 emit [char] J emit
  ansi-term::csi [char] H emit
;

end-compress-flash

\ Reboot
reboot
