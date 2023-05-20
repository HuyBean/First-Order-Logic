 % Define a rule to find the length of a list
length([], 0).
length([_|T], N) :-
  length(T, N1),
  N is N1 + 1.
  
% Query the length rule
?- length([1, 2, 3, 4, 5], Length).