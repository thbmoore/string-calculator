# string-calculator
StringCalculator.exe is a command line application for evaluating mathematical expressions entered as text. For example:

```
>StringCalculator (3+1)*(4-6/3)
(3+1)*(4-6/3)
 = (4*(4-6/3))
 = (4*2)
 = 8 
```

The calculator supports multiplication, division, addition and subtraction.
Grouping with parentheses is also supported. All numbers are treated as integers.

Any number of command line arguments are accepted. The arguments will be concatenated to form the
input expression. Thus, spaces are allowed in the input.  

```
>StringCalculator 1 + 1
1+1
 = 2
```

Only numbers, operators, and parentheses are considered valid input.

```
>StringCalculator A+B
Input expression contains invalid characters:
   A
   B
   ```
