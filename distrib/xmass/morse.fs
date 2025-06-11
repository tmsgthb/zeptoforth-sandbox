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

\ morse

compile-to-flash

begin-module morse

  led import
  pin import
  serial import
  internal import

  15 constant MORSE-PIN 

  true constant m-output?

  : m-pin-1 high MORSE-PIN pin! ;

  : m-pin-0 low MORSE-PIN pin! ;

  300 constant TIMEOUT

  : ti 
    m-pin-1 TIMEOUT ms m-pin-0 TIMEOUT ms 
    [ m-output? ] [if] 46 emit [then] 
  ;

  : ta 
    m-pin-1 TIMEOUT 3 * ms m-pin-0 TIMEOUT ms 
    [ m-output? ] [if] 45 emit [then]
  ;

  : betu-end 
    TIMEOUT ms
    [ m-output? ] [if] bl emit [then]
  ;

  : morse-space 
    TIMEOUT ms 
    [ m-output? ] [if] 32 emit 32 emit [then]
  ;

  : morse-a [ m-output? ] [if] [char] a emit [then] ti ta betu-end ;
    
  : morse-b [ m-output? ] [if] [char] b emit [then] ta ti ti ti betu-end ;

  : morse-c [ m-output? ] [if] [char] c emit [then]  ta ti ta ti betu-end ;
    
  : morse-d [ m-output? ] [if] [char] d emit [then]  ta ti ti betu-end ;
    
  : morse-e [ m-output? ] [if] [char] e emit [then]  ti betu-end ;
    
  : morse-f [ m-output? ] [if] [char] f emit [then]  ti ti ta ti betu-end ;
    
  : morse-g [ m-output? ] [if] [char] g emit [then]  ta ta betu-end ;
    
  : morse-h [ m-output? ] [if] [char] h emit [then]  ti ti ti ti betu-end ;

  : morse-i [ m-output? ] [if] [char] i emit [then]  ti ti betu-end ;
    
  : morse-j [ m-output? ] [if] [char] j emit [then]  ti ta ta ta betu-end ;
      
  : morse-k [ m-output? ] [if] [char] k emit [then]  ta ti ta betu-end ;
    
  : morse-l [ m-output? ] [if] [char] l emit [then]  ti ta ti ti betu-end ;
  
  : morse-m  [ m-output? ] [if] [char] m emit [then] ta ta betu-end ;
    
  : morse-n [ m-output? ] [if] [char] n emit [then]  ta ti betu-end ;
    
  : morse-o [ m-output? ] [if] [char] o emit [then]  ta ta ta betu-end ;
    
  : morse-p [ m-output? ] [if] [char] p emit [then]  ti ta ta ti betu-end ;
    
  : morse-q [ m-output? ] [if] [char] q emit [then]  ta ta ti ta betu-end ;
    
  : morse-r [ m-output? ] [if] [char] r emit [then]  ti ta ti betu-end ;
    
  : morse-s [ m-output? ] [if] [char] s emit [then]  ti ti ti betu-end ;
    
  : morse-t [ m-output? ] [if] [char] t emit [then]  ta betu-end ;
    
  : morse-u [ m-output? ] [if] [char] u emit [then]  ti ti ta betu-end ;
    
  : morse-v [ m-output? ] [if] [char] v emit [then]  ti ti ti ta betu-end ;

  : morse-w [ m-output? ] [if] [char] w emit [then]  ti ta ta betu-end ;

  : morse-x [ m-output? ] [if] [char] x emit [then]  ta ti ti ta betu-end ;

  : morse-y [ m-output? ] [if] [char] y emit [then]  ta ti ta ta betu-end ;

  : morse-z [ m-output? ] [if] [char] z emit [then]  ta ta ti ti betu-end ;

  : morse-0 [ m-output? ] [if] [char] 0 emit [then]  ta ta ta ta ta betu-end ;
  
  : morse-1 [ m-output? ] [if] [char] 1 emit [then]  ti ta ta ta ta betu-end ;
  
  : morse-2 [ m-output? ] [if] [char] 2 emit [then]  ti ti ta ta ta betu-end ;

  : morse-3 [ m-output? ] [if] [char] 3 emit [then]  ti ti ti ta ta betu-end ;

  : morse-4 [ m-output? ] [if] [char] 4 emit [then]  ti ti ti ti ta betu-end ;

  : morse-5 [ m-output? ] [if] [char] 5 emit [then]  ti ti ti ti ti betu-end ;

  : morse-6 [ m-output? ] [if] [char] 6 emit [then]  ta ti ti ti ti betu-end ;

  : morse-7 [ m-output? ] [if] [char] 7 emit [then]  ta ta ti ti ti betu-end ;

  : morse-8 [ m-output? ] [if] [char] 8 emit [then]  ta ta ta ti ti betu-end ;
  
  : morse-9 [ m-output? ] [if] [char] 9 emit [then]  ta ta ta ta ti betu-end ;

  : :morse ( "name" "code" -- )
    token
    dup 0= triggers x-token-expected
    start-compile
    begin
      source >parse @ > if
	      >parse @ + c@ 1 >parse +!
	      case
	        [char] a of postpone morse-a false endof
          [char] b of postpone morse-b false endof
          [char] c of postpone morse-c false endof
          [char] d of postpone morse-d false endof
          [char] e of postpone morse-e false endof
          [char] f of postpone morse-f false endof
          [char] g of postpone morse-g false endof
          [char] h of postpone morse-h false endof
          [char] i of postpone morse-i false endof
          [char] j of postpone morse-j false endof
          [char] k of postpone morse-k false endof
          [char] l of postpone morse-l false endof
          [char] m of postpone morse-m false endof
          [char] n of postpone morse-n false endof
          [char] o of postpone morse-o false endof
          [char] p of postpone morse-p false endof
          [char] q of postpone morse-q false endof
          [char] r of postpone morse-r false endof
          [char] s of postpone morse-s false endof
          [char] t of postpone morse-t false endof
          [char] u of postpone morse-u false endof
          [char] v of postpone morse-v false endof
          [char] w of postpone morse-w false endof
          [char] x of postpone morse-x false endof
          [char] y of postpone morse-y false endof
          [char] z of postpone morse-z false endof
          \ [char] 0 of postpone morse-0 false endof
          \ [char] 1 of postpone morse-1 false endof
          \ [char] 2 of postpone morse-2 false endof
          \ [char] 3 of postpone morse-3 false endof
          \ [char] 4 of postpone morse-4 false endof
          \ [char] 5 of postpone morse-5 false endof
          \ [char] 6 of postpone morse-6 false endof
          \ [char] 7 of postpone morse-7 false endof
          \ [char] 8 of postpone morse-8 false endof
          \ [char] 9 of postpone morse-9 false endof
          32 of postpone morse-space false endof
          [char] ; of true endof
          false swap
	      endcase
      else
	      drop prompt-hook @ ?execute refill false
      then
      until
      visible
      end-compile,
  ;

  : init-morser
    serial-console    
    MORSE-PIN output-pin
    green toggle-led
    m-pin-1
    500 ms
    green toggle-led
    m-pin-0
  ;
    
end-module


