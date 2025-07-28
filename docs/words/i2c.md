# I2C Words

zeptoforth includes support for I2C peripherals on the RP2040 (e.g. the Raspberry Pi Pico) and the RP2350 (e.g.. the Raspberry Pi Pico 2). It supports both master and slave modes of operation on two different I2C peripheral devices, numbered 0 and 1. It supports clocks up to 1 MHz, as that is the limit supported by the RP2040's I2C peripherals, and defaults to a clock of 400 kHz. Note that it does not currently support general calls or slaves sending NACK, and handling of interrupted sends when multiple masters are in use must be handled manually by the user.

### `i2c`

The `i2c` module contains the following words:

##### `enable-i2c`
( i2c -- )

Enable an I2C peripheral. Note that multiple nested enablings are permitted.

##### `disable-i2c`
( i2c -- )

Disable an I2C peripheral. Note that multiple nested disablings are permitted.

##### `i2c-clock!`
( clock i2c -- )

Set the clock for an I2C peripheral. Note that for most purposes a clock of 400000 is recommended, and it is this value to which the clock of an I2C peripheral defaults.

##### `i2c-alternate`
( i2c -- alternate )

Get the alternate function for an I2C peripheral.

##### `i2c-pin`
( i2c pin -- )

Configure a pin to be an I2C pin. In releases more recent than 1.14.1 this will set the pin to be an internal pull-up per the RP2040 and RP2350 datasheets/reference manuals. Previously it did not change the pull-up/pull-down configuration of the pin.

##### `master-i2c`
( i2c -- )

Set an I2C peripheral to be a master.

##### `slave-i2c`
( i2c -- )

Set an I2C peripheral to be a slave.

##### `7-bit-i2c-addr`
( i2c -- )

Set an I2C peripheral to use 7-bit I2C addresses.

##### `10-bit-i2c-addr`
( i2c -- )

Set an I2C peripheral to use 10-bit I2C addresses.

##### `i2c-target-addr!`
( i2c-addr i2c -- )

Set the target address of an I2C peripheral.

##### `i2c-slave-addr!`
( i2c-addr i2c -- )

Set the address of a slave I2C peripheral.

##### `wait-i2c-master`
( i2c -- accepted )

Wait on a slave I2C peripheral for communication from an I2C master, and return either `accept-send` if the I2C master initiates sending data or `accept-recv` if the I2C master initiates receiving data. If `timeout` (in the `task` module) is set to a value other than `no-timeout` a timeout in ticks (usually 100 µs per tick) is waited for, where if no communication is initiated before the timeout expires, `x-timed-out` is raised.

##### `wait-i2c-master-send`
( i2c -- )

Wait on a slave I2C peripheral for the initiation of sending data from an I2C master. If `timeout` (in the `task` module) is set to a value other than `no-timeout` a timeout in ticks (usually 100 µs per tick) is waited for, where if no send is initiated before the timeout expires, `x-timed-out` is raised.

##### `wait-i2c-master-recv`
( i2c -- )

Wait on a slave I2C peripheral for the initiation of receiving data from an I2C master. If `timeout` (in the `task` module) is set to a value other than `no-timeout` a timeout in ticks (usually 100 µs per tick) is waited for, where if no receive is initiated before the timeout expires, `x-timed-out` is raised.

##### `>i2c`
( c-addr u i2c -- u' )

Send data from a buffer defined by *c-addr* and *u* bytes over an I2C peripheral. The number of bytes actually sent are returned. If `timeout` (in the `task` module) is set to a value other than `no-timeout` a timeout in ticks (usually 100 µs per tick) is waited for, where if sending is not complete before the timeout expires, `x-timed-out` is raised.

##### `>i2c-stop`
( c-addr u i2c -- u' )

Send data from a buffer defined by *c-addr* and *u* bytes over an I2C peripheral and signal a STOP condition once sending is complete. The number of bytes actually sent are returned. If `timeout` (in the `task` module) is set to a value other than `no-timeout` a timeout in ticks (usually 100 µs per tick) is waited for, where if sending is not complete before the timeout expires, `x-timed-out` is raised.

##### `>i2c-restart`
( c-addr u i2c -- u' )

Send data from a buffer defined by *c-addr* and *u* bytes over an I2C peripheral and signal a RESTART condition at the beginning of sending. The number of bytes actually sent are returned. If `timeout` (in the `task` module) is set to a value other than `no-timeout` a timeout in ticks (usually 100 µs per tick) is waited for, where if sending is not complete before the timeout expires, `x-timed-out` is raised.

##### `>i2c-restart-stop`
( c-addr u i2c -- u' )

Send data from a buffer defined by *c-addr* and *u* bytes over an I2C peripheral, signaling a RESTART condition at the beginning of sending, and signal a STOP condition once sending is complete.The number of bytes actually sent are returned.  If `timeout` (in the `task` module) is set to a value other than `no-timeout` a timeout in ticks (usually 100 µs per tick) is waited for, where if sending is not complete before the timeout expires, `x-timed-out` is raised.

##### `i2c>`
( c-addr u i2c -- u' )

Receive data into a buffer defined by *c-addr* and *u* bytes from an I2C peripheral. The number of bytes actually received are returned. If `timeout` (in the `task` module) is set to a value other than `no-timeout` a timeout in ticks (usually 100 µs per tick) is waited for, where if receiving is not complete before the timeout expires, `x-timed-out` is raised.

##### `i2c-stop>`
( c-addr u i2c -- u' )

Receive data into a buffer defined by *c-addr* and *u* bytes from an I2C peripheral and signal a STOP condition once receiving is complete. The number of bytes actually received are returned. If `timeout` (in the `task` module) is set to a value other than `no-timeout` a timeout in ticks (usually 100 µs per tick) is waited for, where if receiving is not complete before the timeout expires, `x-timed-out` is raised.

##### `i2c-restart>`
( c-addr u i2c -- u' )

Receive data into a buffer defined by *c-addr* and *u* bytes from an I2C peripheral and signal a RESTART condition at the beginning of receiving. The number of bytes actually received are returned. If `timeout` (in the `task` module) is set to a value other than `no-timeout` a timeout in ticks (usually 100 µs per tick) is waited for, where if receiving is not complete before the timeout expires, `x-timed-out` is raised.

##### `i2c-restart-stop>`
( c-addr u i2c -- u' )

Receive data into a buffer defined by *c-addr* and *u* bytes from an I2C peripheral, signaling a RESTART condition at the beginning of receiving, and signal a STOP condition once receiving is complete. The number of bytes actually received are returned. If `timeout` (in the `task` module) is set to a value other than `no-timeout` a timeout in ticks (usually 100 µs per tick) is waited for, where if receiving is not complete before the timeout expires, `x-timed-out` is raised.

##### `i2c-nack`
( i2c -- )

Set the next byte received by a slave I2C peripheral to be responded to with a NACK.

##### `clear-i2c`
( i2c -- )

Reset the state of an I2C peripheral.

##### `accept-send`
( -- constant )

Constant returned by `wait-i2c-master` on accepting a send from an I2C master.

##### `accept-recv`
( -- constant )

Constant returned by `wait-i2c-master` on accepting a receive from an I2C master.

##### `x-out-of-range-clock`
( -- )

Exception raised if an out of range clock is set with `i2c-clock!`.

##### `x-i2c-target-noack`
( -- )

Exception raised if an I2C target address is not acknowledged.

##### `x-arb-lost`
( -- )

Exception raised on arbitration lost.

##### `x-i2c-tx-error`
( -- )

Exception raised on transmission error during sending.

##### `x-i2c-rx-over`
( -- )

Exception raised on receive overflow; note that this should never happen, as this I2C driver configures the I2C peripherals to use clock stretching in case of RX FIFO full state.

##### `x-invalid-i2c`
( -- )

Exception raised on specifying an invalid I2C peripheral index (i.e. one other than 0 or 1).

##### `x-out-of-range-addr`
( -- )

Exception raised on specifying an invalid I2C address.

##### `x-invalid-op-for-master-mode`
( -- )

Exception raised on a slave attempting to carry out an operation which is inappropriate for the state the master is in.

##### `x-invalid-op-for-slave`
( -- )

Exception raised on a slave attempting to carry out an operation not permitted for slaves.

##### `x-invalid-op-for-master`
( -- )

Exception raised on a master attempting to carry out an operation not permitted for masters.

##### `x-master-not-ready`
( -- )

Exception for if a send or receive is attempted by a slave and the master is not in an appropriate state.
