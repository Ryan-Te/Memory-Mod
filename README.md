# Memory-Mod
A TUNG mod which adds components that hold RAM or ROM inside them  
  
This mod is pretty simple to use, so the only thing I need to explain is flashing RAM and ROM  
  
you can create a ram file like this  
```
MMROM
b, t;
DATA
...
...
...
```  
b is the base  
use D for Decimal,  
H for Hexadecimal,  
B for Binary and  
O for Octal.  

t is the format.  

if you remove t and the comma,  it will format the rom to be based on n digits  
8 for binary,  
3 for octal,  
3 for decimal and  
2 for hex.  

if t is "lb" then every line break means a new value  
if t is "lbs" then every line break and space means a new value  

in lb and lbs format, if you put more digits than it expects then they will be cut off.  
if you put less, it will act as if there is invisible 0s behind it.  
thats pretty much it for creating these files.  
  
to load it you do `setRomLoc LOCATION`. LOCATION is with the TUNG install directory as the root.  
then you pulse the `Change Flash Location` peg and then pulse the `Flash ROM` or `Flash RAM` peg.  

if you pulse the `Save RAM` peg on the RAM's it will make a MMROM file with the current RAM data in it and save it to the location set by `setRomLoc` 

Pin Reference:  
