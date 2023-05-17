% Early definition
:- discontiguous male/1, female/1, parent/2, married/2, divorced/2.

% General male gender
male('Prince Phillip').
male('Prince Charles').
male('Captain Mark Phillips').
male('Timothy Laurence').
male('Prince Andrew').
male('Prince Edward').
male('Prince William').
male('Prince Harry').
male('Peter Phillips').
male('Mike Tindall').
male('James, Viscount Severn').
male('Prince George').

% General female gender
female('Queen Elizabeth II').
female('Princess Diana').
female('Camilla Parker Bowles').
female('Princess Anne').
female('Sarah Ferguson').
female('Kate Middleton').
female('Autumn Kelly').
female('Sophie Rhys-jones').
female('Zara Phillips').
female('Princess Beatrice').
female('Princess Eugenie').
female('Lady Louise Mountbatten-Windsor').
female('Princess Charlotte').
female('Savannah Phillips').
female('Isla Phillips').
female('Mia Grace Tindall').

% FAMILY SITUATION

% Queen Elizabeth II and Prince Phillip 
married('Prince Phillip', 'Queen Elizabeth II').
parent('Queen Elizabeth II','Prince Charles').
parent('Prince Phillip', 'Prince Charles').
parent('Queen Elizabeth II','Princess Anne').
parent('Prince Phillip', 'Princess Anne').
parent('Queen Elizabeth II','Prince Andrew').
parent('Prince Phillip', 'Prince Andrew').
parent('Queen Elizabeth II','Prince Edward').
parent('Prince Phillip', 'Prince Edward').

% Prince Charles and Princess Diana and Camilla Parker Bowles
divorced('Prince Charles', 'Princess Diana').
married('Camilla Parker Bowles', 'Prince Charles').
parent('Prince Charles', 'Prince William').
parent('Princess Diana', 'Prince William').
parent('Prince Charles', 'Prince Harry').
parent('Princess Diana', 'Prince Harry').

% Princess Anne and Captain Mark Phillips and Timothy Laurence
divorced('Captain Mark Phillips', 'Princess Anne').
married('Timothy Laurence', 'Princess Anne').
parent('Princess Anne', 'Peter Phillips').
parent('Captain Mark Phillips', 'Peter Phillips').
parent('Princess Anne', 'Zara Phillips').
parent('Captain Mark Phillips', 'Zara Phillips').

% Prince Andrew and Sarah Ferguson
divorced('Prince Andrew', 'Sarah Ferguson').
parent('Prince Andrew', 'Princess Beatrice').
parent('Sarah Ferguson', 'Princess Beatrice').
parent('Prince Andrew', 'Princess Eugenie').
parent('Sarah Ferguson', 'Princess Eugenie').

% Prince Edward and Sophie Rhys-jones
married('Prince Edward', 'Sophie Rhys-jones').
parent('Prince Edward', 'James, Viscount Severn').
parent('Sophie Rhys-jones', 'James, Viscount Severn').
parent('Prince Edward', 'Lady Louise Mountbatten-Windsor').
parent('Sophie Rhys-jones', 'Lady Louise Mountbatten-Windsor').

% Prince William and Kate Middleton
married('Kate Middleton', 'Prince William').
parent('Kate Middleton', 'Prince George').
parent('Prince William', 'Prince George').
parent('Kate Middleton', 'Princess Charlotte').
parent('Prince William', 'Princess Charlotte').

% Peter Phillips and Autumn Kelly
married('Peter Phillips', 'Autumn Kelly').
parent('Peter Phillips', 'Savannah Phillips').
parent('Autumn Kelly', 'Savannah Phillips').
parent('Peter Phillips', 'Isla Phillips').
parent('Autumn Kelly', 'Isla Phillips').

% Zara Phillips and Mike Tindall
married('Zara Phillips', 'Mike Tindall').
parent('Mike Tindall', 'Mia Grace Tindall').
parent('Zara Phillips', 'Mia Grace Tindall').

%
% Addition predicates
%

% if a person is male and married to Wife then he is a husband of Wife.
husband(Person, Wife) :- male(Person), married(Person, Wife).

% if a person is female and married to Husband then she is a wife of Husband.
wife(Person, Husband) :- female(Person), married(Person, Husband).

% if Parent is male and Parent is a parent of Child then Parent is a father of Child.
father(Parent, Child) :- male(Parent), parent(Parent, Child).

% if Parent is female and Parent is a parent of Child then Parent is a mother of Child.
mother(Parent, Child) :- female(Parent), parent(Parent, Child).

% if Parent is a parent of Child then Child is a child of Parent.
child(Child, Parent) :- parent(Parent, Child).

% if Child is male and Child is child of Parent then Child is son of Parent.
son(Child, Parent) :- male(Child), child(Child, Parent).

% if Child is female and Child is child of Parent then Child is daughter of Parent.
daughter(Child, Parent) :- female(Child), child(Child, Parent).

% if X divorced Y then Y also divorced X.
divorced(X, Y) :- divorced(Y, X).

% if X married Y then Y also married X.
married(X, Y) :- married(Y, X).

% if GP is parent of Parent and Parent is parent of GC 
% then GP is grandparent of GC. 
grandparent(GP, GC) :- parent(GP, Parent), parent(Parent, GC).

% if GM is female and GM is grandparent of GC 
% then GM is grandmother of GC.
grandmother(GM, GC) :- female(GM), grandparent(GM, GC).

% if GF is male and GM is grandparent of GC 
% then GF is grandmother of GC.
grandfather(GF, GC) :- male(GF), grandparent(GF, GC).

% if GP is grandparent of GC then GC is grandchild of GP. 
grandchild(GC, GP) :- grandparent(GP, GC).

% if GS is male and GS is grandchild of GP then GS is grandson of GP.
grandson(GS, GP) :- male(GS), grandchild(GS, GP).

% if GD is female and GD is grandchild of GP then GD is granddaughter of GP.
granddaughter(GD, GP) :- female(GD), grandchild(GD, GP).

% if 2 different Person have the same mother and father then they are sibling.
sibling(Person1, Person2) :- father(Father, Person1), father(Father, Person2), mother(Mother, Person1), mother(Mother, Person2), Person1 \= Person2.

% if a Person is male and Sibling is sibling of Person then Person is a brother of Sibling.
brother(Person, Sibling) :- male(Person), sibling(Person, Sibling).

% if a Person is female and Sibling is sibling of Person then Person is a sister of Sibling.
sister(Person, Sibling) :- female(Person), sibling(Person, Sibling).

% if Parent is parent of NieceNephew and [Person is sister of Parent or (Uncle is brother of Parent and Person is wife of Uncle)]
% then Person is aunt of NieceNephew.
aunt(Person, NieceNephew) :- parent(Parent, NieceNephew), (sister(Person, Parent); (brother(Uncle, Parent), wife(Person, Uncle))).

% if Parent is parent of NieceNephew and [Person is brother of Parent or (Aunt is sister of Parent and Person is husband of Aunt)]
% then Person is aunt of NieceNephew.
uncle(Person, NieceNephew) :- parent(Parent, NieceNephew), (brother(Person, Parent); (sister(Aunt, Parent), husband(Person, Aunt))).

% if Person is female and AuntUncle is aunt or uncle of Person then Person is niece of AuntUncle.
niece(Person, AuntUncle) :- female(Person), (aunt(AuntUncle, Person); uncle(AuntUncle, Person)).

% if Person is male and AuntUncle is aunt or uncle of Person then Person is nephew of AuntUncle.
nephew(Person, AuntUncle) :- male(Person), (aunt(AuntUncle, Person); uncle(AuntUncle, Person)).



