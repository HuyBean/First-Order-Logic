% Early definition
:- discontiguous NextLevel/2.

% General knowledge base
NextLevel('Animal', 'Mammal').
NextLevel('Animal', 'Bird').
NextLevel('Animal', 'Reptile').

% Mammal Class
NextLevel('Mammal', 'Primate').
NextLevel('Mammal', 'Cetacean').
NextLevel('Mammal', 'Carnivore').

NextLevel('Primate', 'Hominidae').
NextLevel('Primate', 'Hylobatidae').

NextLevel('Hominidae', 'Human').
NextLevel('Hominidae', 'Chimpanzee').
NextLevel('Hominidae', 'Gorilla').

NextLevel('Hylobatidae', 'Gibbon').
NextLevel('Hylobatidae', 'Siamang').

NextLevel('Cetacean', 'Delphinidae').
NextLevel('Cetacean', 'Balaenidae').
NextLevel('Cetacean', 'Ziphiidae').

NextLevel('Delphinidae', 'Dolphin').
NextLevel('Balaenidae', 'Whale').
NextLevel('Ziphiidae', 'Sea lion').

NextLevel('Carnivore', 'Felidae').
NextLevel('Carnivore', 'Canidae').

NextLevel('Felidae', 'Tiger').
NextLevel('Felidae', 'Lion').
NextLevel('Felidae', 'Panther').
NextLevel('Felidae', 'Panthera pardus').

NextLevel('Canidae', 'Wolf').
NextLevel('Canidae', 'Dog').
NextLevel('Canidae', 'Panthera pardus').

%Bird Class
NextLevel('Bird', 'Passeriformes').
NextLevel('Bird', 'Falconiformes').
NextLevel('Bird', 'Piciformes').
NextLevel('Bird', 'Anseriformes').
NextLevel('Bird', 'Apterygiformes').
NextLevel('Bird', 'Trochilliformes').

NextLevel('Passeriformes', 'Muscicapidae').
NextLevel('Passeriformes','Mimidae').

NextLevel('Muscicapidae', 'Robin').
NextLevel('Muscicapidae', 'Goldfinch').

NextLevel('Mimidae', 'Mockingbird').

NextLevel('Falconiformes', 'Falconidae').
NextLevel('Falconiformes', 'Preyingbirds').
NextLevel('Falconiformes', 'Accipitridae').

NextLevel('Falconidae', 'Gyfalcon').

NextLevel('Preyingbirds', 'Black vulture').
NextLevel('Preyingbirds', 'Osprey').

NextLevel('Accipitridae', 'Hawk').

NextLevel('Piciformes', 'Honeyguide').
NextLevel('Piciformes', 'Woodpecker').
NextLevel('Piciformes', 'Barbet').

NextLevel('Honeyguide', 'Spotted Honeyguide').
NextLevel('Honeyguide', 'Least Honeyguide').

NextLevel('Woodpecker', 'Red-headed').
NextLevel('Woodpecker', 'Hairy').
NextLevel('Woodpecker', 'Downy').

NextLevel('Barbet', 'Coppersmith').

NextLevel('Anseriformes', 'Waterfowl').

NextLevel('Waterfowl', 'Mottled duck').
NextLevel('Waterfowl', 'Mandarin duck').
NextLevel('Waterfowl', 'Ruddy duck').

NextLevel('Apterygiformes', 'Flightless birds').

NextLevel('Flightless birds', 'Ruddy duck').
NextLevel('Flightless birds', 'Ostrich').
NextLevel('Flightless birds', 'Kiwi').
NextLevel('Flightless birds', 'Penguin').

NextLevel('Trochilliformes', 'Hummingbirds').

NextLevel('Hummingbirds', 'Bumblebee').
NextLevel('Hummingbirds', 'Ruby-throated').
NextLevel('Hummingbirds', 'Anna').

NextLevel('Reptile', 'Testudines').
NextLevel('Reptile', 'Squamata').
NextLevel('Reptile', 'Crocodylia').

NextLevel('Testudines', 'Emydidae').
NextLevel('Testudines', 'Geoemydidae').
NextLevel('Testudines', 'Dermochelyidae').

NextLevel('Emydidae', 'Red-eared slider').
NextLevel('Emydidae', 'Painted turtle').

NextLevel('Geoemydidae', 'Chinese pond turtle').

NextLevel('Dermochelyidae', 'Leatherback sea turtle').

NextLevel('Squamata', 'Gekkomidae').
NextLevel('Squamata', 'Pythonidae').

NextLevel('Gekkomidae', 'Leopard gecko').
NextLevel('Gekkomidae', 'House gecko').

NextLevel('Pythonidae', 'Reticulated python').
NextLevel('Pythonidae', 'Green python').

NextLevel('Crocodylia', 'Crocodylidae').
NextLevel('Crocodylia', 'Alligatoridae').
NextLevel('Crocodylia', 'Gavialidae').

NextLevel('Crocodylidae', 'American crocodile').
NextLevel('Crocodylidae', 'Australian crocodile').

NextLevel('Alligatoridae', 'American alligator').
NextLevel('Alligatoridae', 'Australian alligator').

NextLevel('Gavialidae', 'Gharial').

%Addition predicates

%if that Animal is not belonged to Family, Order, Class, Kingdom.
Family_of(Family, Animal) :-	
Order_of(Order, Animal) :-
Class_of(Class, Animal)	:-	
Kingdom_of(Kingdom, Animal) :-	

%if the Animal1 is same feature with the Animal2.
IsSameFamily(Animal1, Animal2) :-		
IsSameOrder(Animal1, Animal2) :-		
IsSameClass(Animal1, Animal2) :-	

%if the Animal has more than 1 feature.
BothFamily(Animal) :-
BothOrder(Animal) :-		
BothClass(Animal) :-	

%if the name in () is Species or Kingdom or Order or Class or Family.
IsSpecies(Animal) :-	
IsKingdom(Animal) :-	
IsClass(Animal) :-
IsOrder(Animal) :-
IsFamily(Animal) :-	

%if 2 animal can compete or eat each other.
CanCompete(Animal1, Animal2) :-		
CanEat(Mammal, Bird) :-