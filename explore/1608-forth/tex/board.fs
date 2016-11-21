\ board definitions

\ eraseflash
compiletoflash
( board start: ) here dup hex.

\ TODO this should have been in hal.fs ...
RCC $18 + constant RCC-APB2ENR

: flash-kb ( -- u )  \ return size of flash memory in KB
  $1FFFF7E0 h@ ;

: -jtag ( -- )  \ disable JTAG on PB3 PB4 PA15
  25 bit AFIO-MAPR bis! ;

: list ( -- )  \ list all words in dictionary, short form
  cr dictionarystart begin
      dup 6 + ctype space
        dictionarynext until drop ;

\ some clock utilities such as systick-hz have not been loaded yet
include x-clock.fs

include ../mlib/cond.fs
include ../mlib/hexdump.fs
include ../flib/stm32f1/io.fs
include ../flib/pkg/pins36.fs
include ../flib/any/i2c-bb.fs
include ../flib/stm32f1/spi.fs

PA1 constant LED

: hello ( -- ) flash-kb . ." KB <tex> " hwid hex. ;

: init ( -- )  \ board initialisation
  init  \ this is essential to start up USB comms!
  -jtag  \ disable JTAG, we only need SWD
  OMODE-PP LED io-mode!
\ hello ." ok." cr
  1000 systick-hz
;

( board end, size: ) here dup hex. swap - .
cornerstone <<<board>>>
