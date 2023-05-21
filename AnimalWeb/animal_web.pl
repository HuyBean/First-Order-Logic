% Early definition

% General knowledge base
nextlevel('Animal', 'Mammal').
nextlevel('Animal', 'Bird').
nextlevel('Animal', 'Reptile').

% Mammal Class
nextlevel('Mammal', 'Primate').
nextlevel('Mammal', 'Cetacean').
nextlevel('Mammal', 'Carnivore').

nextlevel('Primate', 'Hominidae').
nextlevel('Primate', 'Hylobatidae').

nextlevel('Hominidae', 'Human').
nextlevel('Hominidae', 'Chimpanzee').
nextlevel('Hominidae', 'Gorilla').

nextlevel('Hylobatidae', 'Gibbon').
nextlevel('Hylobatidae', 'Siamang').

nextlevel('Cetacean', 'Delphinidae').
nextlevel('Cetacean', 'Balaenidae').
nextlevel('Cetacean', 'Ziphiidae').

nextlevel('Delphinidae', 'Dolphin').
nextlevel('Balaenidae', 'Whale').
nextlevel('Ziphiidae', 'Sea lion').

nextlevel('Carnivore', 'Felidae').
nextlevel('Carnivore', 'Canidae').

nextlevel('Felidae', 'Tiger').
nextlevel('Felidae', 'Lion').
nextlevel('Felidae', 'Panther').
nextlevel('Felidae', 'Panthera pardus').

nextlevel('Canidae', 'Wolf').
nextlevel('Canidae', 'Dog').
nextlevel('Canidae', 'Panthera pardus').

%Bird Class
nextlevel('Bird', 'Passeriformes').
nextlevel('Bird', 'Falconiformes').
nextlevel('Bird', 'Piciformes').
nextlevel('Bird', 'Anseriformes').
nextlevel('Bird', 'Apterygiformes').
nextlevel('Bird', 'Trochilliformes').

nextlevel('Passeriformes', 'Muscicapidae').
nextlevel('Passeriformes','Mimidae').

nextlevel('Muscicapidae', 'Robin').
nextlevel('Muscicapidae', 'Goldfinch').

nextlevel('Mimidae', 'Mockingbird').

nextlevel('Falconiformes', 'Falconidae').
nextlevel('Falconiformes', 'Preyingbirds').
nextlevel('Falconiformes', 'Accipitridae').

nextlevel('Falconidae', 'Gyfalcon').

nextlevel('Preyingbirds', 'Black vulture').
nextlevel('Preyingbirds', 'Osprey').

nextlevel('Accipitridae', 'Hawk').

nextlevel('Piciformes', 'Honeyguide').
nextlevel('Piciformes', 'Woodpecker').
nextlevel('Piciformes', 'Barbet').

nextlevel('Honeyguide', 'Spotted Honeyguide').
nextlevel('Honeyguide', 'Least Honeyguide').

nextlevel('Woodpecker', 'Red-headed').
nextlevel('Woodpecker', 'Hairy').
nextlevel('Woodpecker', 'Downy').

nextlevel('Barbet', 'Coppersmith').

nextlevel('Anseriformes', 'Waterfowl').

nextlevel('Waterfowl', 'Mottled duck').
nextlevel('Waterfowl', 'Mandarin duck').
nextlevel('Waterfowl', 'Ruddy duck').

nextlevel('Apterygiformes', 'Flightless birds').

nextlevel('Flightless birds', 'Ruddy duck').
nextlevel('Flightless birds', 'Ostrich').
nextlevel('Flightless birds', 'Kiwi').
nextlevel('Flightless birds', 'Penguin').

nextlevel('Trochilliformes', 'Hummingbirds').

nextlevel('Hummingbirds', 'Bumblebee').
nextlevel('Hummingbirds', 'Ruby-throated').
nextlevel('Hummingbirds', 'Anna').

nextlevel('Reptile', 'Testudines').
nextlevel('Reptile', 'Squamata').
nextlevel('Reptile', 'Crocodylia').

nextlevel('Testudines', 'Emydidae').
nextlevel('Testudines', 'Geoemydidae').
nextlevel('Testudines', 'Dermochelyidae').

nextlevel('Emydidae', 'Red-eared slider').
nextlevel('Emydidae', 'Painted turtle').

nextlevel('Geoemydidae', 'Chinese pond turtle').

nextlevel('Dermochelyidae', 'Leatherback sea turtle').

nextlevel('Squamata', 'Gekkomidae').
nextlevel('Squamata', 'Pythonidae').

nextlevel('Gekkomidae', 'Leopard gecko').
nextlevel('Gekkomidae', 'House gecko').

nextlevel('Pythonidae', 'Reticulated python').
nextlevel('Pythonidae', 'Green python').

nextlevel('Crocodylia', 'Crocodylidae').
nextlevel('Crocodylia', 'Alligatoridae').
nextlevel('Crocodylia', 'Gavialidae').

nextlevel('Crocodylidae', 'American crocodile').
nextlevel('Crocodylidae', 'Australian crocodile').

nextlevel('Alligatoridae', 'American alligator').
nextlevel('Alligatoridae', 'Australian alligator').

nextlevel('Gavialidae', 'Gharial').

%Addition predicates.

%if the name in () is Species or Kingdom or Order or Class or Family.

%To identify if that name is a Species
isspecies(Animal) :- nextlevel('Animal', Class), nextlevel(Class, Order), nextlevel(Order, Family), nextlevel(Family, Animal).

%To identify if that name is a Kingdom
iskingdom(Animal) :- Animal ==	'Animal'.

%To identify if that name is a Order
isclass(Animal) :- nextlevel('Animal', Animal).

%To identify if that name is a Class
isorder(Animal) :- nextlevel('Animal', Class), nextlevel(Class, Animal).

%To identify if that name is a Family
isfamily(Animal) :-	nextlevel('Animal', Class), nextlevel(Class, Order), nextlevel(Order, Animal).

%if that Animal is not belonged to Family, Order, Class, Kingdom.

%To identify if that Animal is belonged to Family
family_of(Family, Animal) :-	nextlevel(Family, Animal), isfamily(Family).

%To identify if that Animal is belonged to Order
order_of(Order, Animal) :-  nextlevel(Order, Family), family_of(Family, Animal), isorder(Order).
order_of(Order, Animal) :-  isorder(Order), nextlevel(Order,Animal).

%To identify if that Animal is belonged to Class
class_of(Class, Animal)	:-	nextlevel(Class, Order), order_of(Order, Animal), isclass(Class).
class_of(Class, Animal) :-  isclass(Class), nextlevel(Class, Animal).
class_of(Class, Animal) :-  isclass(Class), nextlevel(Class, Order), nextlevel(Order, Animal).

%To identify if that Animal is belonged to Kingdom
kingdom_of(Kingdom, Animal) :-	iskingdom(Kingdom), isclass(Animal).
kingdom_of(Kingdom, Animal) :-	iskingdom(Kingdom), isfamily(Animal).
kingdom_of(Kingdom, Animal) :-	iskingdom(Kingdom), isorder(Animal).
kingdom_of(Kingdom, Animal) :-  iskingdom(Kingdom), isspecies(Animal).


%if the Animal1 is same feature with the Animal2.

%To identify that Animal1 and Animal2 are in the same family
issamefamily(Animal1, Animal2) :- family_of(Family, Animal1), family_of(Family, Animal2).

%To identify that Animal1 and Animal2 are in the same order
issameorder(Animal1, Animal2) :- order_of(Order, Animal1), order_of(Order, Animal2).

%To identify that Animal1 and Animal2 are in the same class
issameclass(Animal1, Animal2) :- class_of(Class, Animal1), class_of(Class, Animal2).

%if the Animal has more than 1 feature.

%To identify that Animal is in two families
bothfamily(Animal) :- family_of(Family1, Animal), family_of(Family2, Animal), Family1 \= Family2.

%To identify that Animal is in two orders
bothorder(Animal) :- order_of(Order1, Animal), order_of(Order2, Animal), Order1 \= Order2.

%To identify that Animal is in two classes
bothclass(Animal) :- class_of(Class1, Animal), class_of(Class2, Animal), Class1 \= Class2.

%if 2 animal can compete or eat each other.

% if that creatures are 'Mammal' and 'Reptile' or they are both from
% the same Class, then they can compete each other

%To identify if that Animal1 can compete with Animal2
cancompete(Mammal, Reptile) :-	class_of(Class1, Mammal), Class1 == 'Mammal', class_of(Class2, Reptile), Class2 == 'Reptile'.
cancompete(Mammal, Reptile) :-  class_of(Class1, Mammal), Class1 == 'Reptile', class_of(Class2, Reptile), Class2 == 'Mammal'.
cancompete(Mammal, Reptile) :-  issameclass(Mammal, Reptile).

% if the first animal is 'Mammal' and the second one is 'Bird'
% Then the first one can eat the second

%To identify if Animal1 can eat Animal2
caneat(Mammal, Bird) :- class_of(Class1, Mammal), Class1 == 'Mammal', class_of(Class2, Bird), Class2 == 'Bird'.
caneat(Mammal, Bird) :- isclass(Mammal), Mammal == 'Mammal', isclass(Bird), Bird == 'Bird'.

%To identify an Animal can fly or not
canflight(Bird):- class_of(Class, Bird), Class == 'Bird', family_of(Family, Bird), Family \= 'Flightless birds', Bird \= 'Ruddy duck'.

