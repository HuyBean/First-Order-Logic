% Define a rule to check if a number is even
even(X) :-
  0 is X mod 2.

% Query the even rule
?- even(4).
?- even(3).