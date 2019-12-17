@ Copyright (c) 2019, Travis Bemann
@ All rights reserved.
@ 
@ Redistribution and use in source and binary forms, with or without
@ modification, are permitted provided that the following conditions are met:
@ 
@ 1. Redistributions of source code must retain the above copyright notice,
@    this list of conditions and the following disclaimer.
@ 
@ 2. Redistributions in binary form must reproduce the above copyright notice,
@    this list of conditions and the following disclaimer in the documentation
@    and/or other materials provided with the distribution.
@ 
@ 3. Neither the name of the copyright holder nor the names of its
@    contributors may be used to endorse or promote products derived from
@    this software without specific prior written permission.
@ 
@ THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
@ AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
@ IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
@ ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE
@ LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
@ CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
@ SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
@ INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
@ CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
@ ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
@ POSSIBILITY OF SUCH DAMAGE.

	@@ Assemble a literal
	define_word "asm-literal", visible_flag
asm_literal:
	push {lr}
	mov r0, tos
	pull_tos
	cmp tos, #0
	blt 1f
	cmp tos, #0xFF
	bgt 2f
	push_tos
	mov tos, r0
	bl _asm_mov_imm
	pop {pc}
2:	push_tos
	mov tos, r0
	bl _asm_ldr_long_imm
	pop {pc}
1:	neg tos, tos
	mov r0, #0xFF
	cmp tos, r0
	bgt 2f
	push_tos
	mov tos, r0
	push {r0}
	bl _asm_mov_imm
	pop {r0}
	push_tos
	mov tos, r0
	push_tos
	mov tos, r0
	bl _asm_neg
	pop {pc}
2:	push_tos
	mov tos, r0
	push {r0}
	bl _asm_ldr_long_imm
	pop {r0}
	push_tos
	mov tos, r0
	push_tos
	mov tos, r0
	bl _asm_neg
	pop {pc}

	@@ Assemble a move immediate instruction
	define_word "asm-mov-imm", visible_flag
_asm_mov_imm:
	push {lr}
	mov r0, tos
	pull_tos
	and r0, #7
	and tos, #0xFF
	lsl r0, #8
	orr tos, r0
	ldr r0, =0x2000
	orr tos, r0
	bl _current_comma_16
	pop {pc}	

	@@ Assemble a long load register immediate pseudo-opcode
	define_word "asm-ldr-long-imm", visible_flag
_asm_ldr_long_imm:
	push {lr}
	mov r0, tos
	pull_tos
	mov r1, tos
	lsr r1, r1, #24
	bne 1f
	push_tos
	mov tos, r0
	bl _asm_ldr_long_imm_1st_zero
	pop {pc}
1:	mov r2, tos
	mv tos, r1
	push_tos
	mv tos, r0
	push {r0, r2}
	bl _asm_mov_imm
	pop {r0, r2}
	mov r1, r2
	lsr r1, r1, #16
	mov r3, #0xFF
	and r1, r3
	bne 1f
	push_tos
	mov tos, r2
	push_tos
	mov tos, r0
	bl _asm_ldr_long_imm_2nd_zero
	pop {pc}
1:	push_tos
	mov tos, #8
	push_tos
	mov tos, r0
	push {r0, r1, r2}
	bl _asm_lsl_imm
	pop {r0, r1, r2}
	push_tos
	mov tos, r1
	push_tos
	mov tos, #0
	push {r0, r2}
	bl _asm_mov_imm
	pop {r0, r2}
	push_tos
	mov tos, #0
	push_tos
	mov tos, r0
	push {r0, r2}
	bl _asm_orr
	pop {r0, r2}
	mov r1, r2
	ldr r1, r1, #8
	mov r3, #0xFF
	and r1, r3
	bne 1f
	push_tos
	mov tos, r2
	push_tos
	mov tos, r0
	bl _asm_ldr_long_imm_3rd_zero
	pop {pc}
1:	push_tos
	mov tos, #8
	push_tos
	mov tos, r0
	push {r0, r1, r2}
	bl _asm_lsl_imm
	pop {r0, r1, r2}
	push_tos
	mov tos, r1
	push_tos
	mov tos, #0
	push {r0, r2}
	bl _asm_mov_imm
	pop {r0, r2}
	push_tos
	mov tos, #0
	push_tos
	mov tos, r0
	push {r0, r2}
	bl _asm_orr
	pop {r0, r2}
	mov r3, #0xFF
	and r2, r3
	bne 1f
	pop {pc}
1:	push_tos
	mov tos, #8
	push_tos
	mov tos, r0
	push {r0, r2}
	bl _asm_lsl_imm
	pop {r0, r2}
	push_tos
	mov tos, r2
	push_tos
	mov tos, #0
	push {r0}
	bl _asm_mov_imm
	pop {r0}
	push_tos
	mov tos, #0
	push_tos
	mov tos, r0
	bl _asm_orr
	pop {pc}


	@@ Assemble a long load register immediate pseudo-opcode
	define_word "asm-ldr-long-imm-1st-zero", visible_flag
_asm_ldr_long_imm_1st_zero:
	push {lr}
	mov r0, tos
	pull_tos
	mov r2, tos
	mov r1, r2
	lsr r1, r1, #16
	mov r3, #0xFF
	and r1, r3
	bne 1f
	push_tos
	mov tos, r2
	push_tos
	mov tos, r0
	bl _asm_ldr_long_imm_1st_2nd_zero
	pop {pc}
1:	push_tos
	mov tos, r1
	push_tos
	mov tos, #0
	push {r0, r2}
	bl _asm_mov_imm
	pop {r0, r2}
	push_tos
	mov tos, #0
	push_tos
	mov tos, r0
	push {r0, r2}
	bl _asm_orr
	pop {r0, r2}
	mov r1, r2
	ldr r1, r1, #8
	mov r3, #0xFF
	and r1, r3
	bne 1f
	push_tos
	mov tos, r2
	push_tos
	mov tos, r0
	bl _asm_ldr_long_imm_1st_3rd_zero
	pop {pc}
1:	push_tos
	mov tos, #8
	push_tos
	mov tos, r0
	push {r0, r1, r2}
	bl _asm_lsl_imm
	pop {r0, r1, r2}
	push_tos
	mov tos, r1
	push_tos
	mov tos, #0
	push {r0, r2}
	bl _asm_mov_imm
	pop {r0, r2}
	push_tos
	mov tos, #0
	push_tos
	mov tos, r0
	push {r0, r2}
	bl _asm_orr
	pop {r0, r2}
	push_tos
	mov tos, #8
	push_tos
	mov tos, r0
	push {r0, r2}
	bl _asm_lsl_imm
	pop {r0, r2}
	mov r3, #0xFF
	and r2, r3
	bne 1f
	pop {pc}
1:	push_tos
	mov tos, r2
	push_tos
	mov tos, #0
	push {r0}
	bl _asm_mov_imm
	pop {r0}
	push_tos
	mov tos, #0
	push_tos
	mov tos, r0
	bl _asm_orr
	pop {pc}

	@@ Assemble a long load register immediate pseudo-opcode
	define_word "asm-ldr-long-imm-2nd-zero", visible_flag
_asm_ldr_long_imm_2nd_zero:
	push {lr}
	mov r0, tos
	pull_tos
	mov r1, tos
	mov r1, r2
	ldr r1, r1, #8
	mov r3, #0xFF
	and r1, r3
	bne 1f
	push_tos
	mov tos, r2
	push_tos
	mov tos, r0
	bl _asm_ldr_long_imm_2nd_3rd_zero
	pop {pc}
1:	push_tos
	mov tos, #16
	push_tos
	mov tos, r0
	push {r0, r1, r2}
	bl _asm_lsl_imm
	pop {r0, r1, r2}
	push_tos
	mov tos, r1
	push_tos
	mov tos, #0
	push {r0, r2}
	bl _asm_mov_imm
	pop {r0, r2}
	push_tos
	mov tos, #0
	push_tos
	mov tos, r0
	push {r0, r2}
	bl _asm_orr
	pop {r0, r2}
	mov r3, #0xFF
	and r2, r3
	bne 1f
	pop {pc}
1:	push_tos
	mov tos, #8
	push_tos
	mov tos, r0
	push {r0, r2}
	bl _asm_lsl_imm
	pop {r0, r2}
	push_tos
	mov tos, r2
	push_tos
	mov tos, #0
	push {r0}
	bl _asm_mov_imm
	pop {r0}
	push_tos
	mov tos, #0
	push_tos
	mov tos, r0
	bl _asm_orr
	pop {pc}

	@@ Assemble a long load register immediate pseudo-opcode
	define_word "asm-ldr-long-imm-1st-2nd-zero", visible_flag
_asm_ldr_long_imm_1st_2nd_zero:
	push {lr}
	mov r0, tos
	pull_tos
	mov r2, tos
	mov r1, r2
	ldr r1, r1, #8
	mov r3, #0xFF
	and r1, r3
	push_tos
	mov tos, r1
	push_tos
	mov tos, #0
	push {r0, r2}
	bl _asm_mov_imm
	pop {r0, r2}
	push_tos
	mov tos, #0
	push_tos
	mov tos, r0
	push {r0, r2}
	bl _asm_orr
	pop {r0, r2}
	push_tos
	mov tos, #8
	push_tos
	mov tos, r0
	push {r0, r2}
	bl _asm_lsl_imm
	pop {r0, r2}
	mov r3, #0xFF
	and r2, r3
	bne 1f
	pop {pc}
1:	push_tos
	mov tos, r2
	push_tos
	mov tos, #0
	push {r0}
	bl _asm_mov_imm
	pop {r0}
	push_tos
	mov tos, #0
	push_tos
	mov tos, r0
	bl _asm_orr
	pop {pc}

	@@ Assemble a long load register immediate pseudo-opcode
	define_word "asm-ldr-long-imm-1st-3rd-zero", visible_flag
_asm_ldr_long_imm_3rd_zero:
	push {lr}
	mov r0, tos
	pull_tos
	mov r2, tos
	mov tos, #8
	push_tos
	mov tos, r0
	push {r0, r2}
	bl _asm_lsl_imm
	pop {r0, r2}
	mov r3, #0xFF
	and r2, r3
	bne 1f
	pop {pc}
1:	push_tos
	mov tos, r2
	push_tos
	mov tos, #0
	push {r0}
	bl _asm_mov_imm
	pop {r0}
	push_tos
	mov tos, #0
	push_tos
	mov tos, r0
	bl _asm_orr
	pop {pc}

	@@ Assemble a long load register immediate pseudo-opcode
	define_word "asm-ldr-long-imm-1st-3rd-zero", visible_flag
_asm_ldr_long_imm_1st_3rd_zero:
	push {lr}
	mov r0, tos
	pull_tos
	mov r2, tos
	mov tos, #16
	push_tos
	mov tos, r0
	push {r0, r2}
	bl _asm_lsl_imm
	pop {r0, r2}
	mov r3, #0xFF
	and r2, r3
	bne 1f
	pop {pc}
1:	push_tos
	mov tos, r2
	push_tos
	mov tos, #0
	push {r0}
	bl _asm_mov_imm
	pop {r0}
	push_tos
	mov tos, #0
	push_tos
	mov tos, r0
	bl _asm_orr
	pop {pc}

	@@ Assemble a long load register immediate pseudo-opcode
	define_word "asm-ldr-long-imm-2nd-3rd-zero", visible_flag
_asm_ldr_long_imm_2nd_3rd_zero:
	push {lr}
	mov r0, tos
	pull_tos
	mov r2, tos
	mov tos, #24
	push_tos
	mov tos, r0
	push {r0, r2}
	bl _asm_lsl_imm
	pop {r0, r2}
	mov r3, #0xFF
	and r2, r3
	bne 1f
	pop {pc}
1:	push_tos
	mov tos, r2
	push_tos
	mov tos, #0
	push {r0}
	bl _asm_mov_imm
	pop {r0}
	push_tos
	mov tos, #0
	push_tos
	mov tos, r0
	bl _asm_orr
	pop {pc}
	
	@@ Assemble an unconditional branch
	define_word "asm-b", visible_flag
_asm_b: push {lr}
	mov r0, tos
	lsr r0, #8
	and r0, #0x7
	orr r0, #0xE0
	and tos, #0xFF
	lsl r0, #8
	orr tos, r0
	bl _current_comma_16
	pop {pc}

	@@ Assemble a branch on equal zero instruction
	define_word "asm-beq", visible_flag
_asm_beq:
	push {lr}
	ldr r0, =0xD000
	and tos, 0xFF
	orr tos, r0
	bl _current_comma_16
	pop {pc}
	
	@@ Assemble a compare to constant instruction
	define_word "asm-cmp-imm", visible_flag
_asm_cmp_imm:
	push {lr}
	mov r0, tos
	pull_tos
	and r0, #7
	and tos, #0xFF
	lsl r0, #8
	orr tos, r0
	ldr r0, =0x2800
	orr tos, r0
	bl _current_comma_16
	pop {pc}
