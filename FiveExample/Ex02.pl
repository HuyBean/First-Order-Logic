% Define a recursive rule to calculate factorial
factorial(0, 1).
factorial(N, Result) :-
 N > 0,
 N1 is N - 1,
 factorial(N1, SubResult),
 Result is N * SubResult.
 
% Query the factorial rule
?- factorial(5, Result).