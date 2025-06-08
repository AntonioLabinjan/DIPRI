# Todo
PROCEDURAL GENERATION SAMO ZA SOBE IZA VRATA, ALI KUHINJE I TO DI SU QUESTOVI NEKA BUDE VAJKA NA ISTIN MISTU
- Napravit smislene okolne zidove i onda to samo sve hitit u škvaru  


NIČ OD TEGA => PREBACIT SE NA QUESTOVE => Glavni svijet se nalazi u jednoj sceni s triggerima koji pokreću mini-igre. Svaki quest/minigra se odvija u zasebnoj sceni bez interakcije s vanjskim svijetom. Prijelaz između scena se vrši putem `SceneManager.LoadScene`, uz mogućnost dodavanja fade efekata za bolji doživljaj.


BOLJE NAPRAVIT JOŠ 1 KAMERU ZA QUEST; UGASIT GLAVNU KAMERU; UPALIT OVU ZA QUEST I ODGIRAT GA TAMO
NE TREBA PRČKAT PO SCENAMA
DISABLEAT PLAYERA DA MI NE DELA GLUPOSTI 




ZANEMARIT SVE PRETHODNO ČA PIŠE, ALI NE OBRISAT ZA SVAKI SLUČAJ

Implementirani questovi: Zaljevanje biljaka & Biljni labirint & Red po brojevima

Triba samo potakivat na UI

Kad testiran questove, unloadat sve scene osim one koja mi triba (i hitit playera na nju da moren interractat)



PROC. GEN. STUFF.
recepcija mora imat attachmentse
hallway s nekoliko različitih elemenata
Prefabs
	osnovni element => par metara hodnika DONE
	kraj hodnika DONE 
	edge hodnika za obe strane DONE
	left turn DONE
	right turn DONE 
	t-shape za hodnika DONE
	križanje DONE 
	prazne sekcije za merge points
	zid, plafon, pod DONE

	
Napisat docks koje zone bi imala koja soba 
Koji itemi bi se našli u kojoj sobi
Any extra rules?

Treba nan plafon
Trebaju nan zidovi
Treba nan docs za generiranje (zone + items)

Trebaju nan prazne sekcije


TODO:
- prazne sekcije za merge
- sekcije unutar soba u kojima se stvari pojavljuju

Pravila za generaciju itema:

WalkieTalkie se pojavljuje samo na recepciji
Putna torba se pojavljuje samo na recepciji
Tacna se pojavljuje samo u baru
Flashlight imaš kod sebe odmah
Nož (not sure dali ga imamo; ako ga nemamo, nema veze, ni 100% nužan) se pojavljuje samo u kuhinji
Rule: ovi navedeni itemi se pojavljuju samo u predviđenim sobama, odnosno ne smiju se pojavit u drugima

Pravila za generaciju(spawn) likova:
Barmen se pojavljuje samo u baru (Bar)
Noćni policajac/čuvar/monster/entity (whatever you decide to call him) bi treba imat podjednaku šansu da se tijekom noći spawna bilo di (any room)
Kuhar se pojavljuje samo u kuhinji (Kitchen)
Portir se pojavljuje na recepciji (Reception) (90% chance) + sobi 404 (Room 404) (10% chance)
Sobarica se pojavljuje u roomsima (imamo 5 tipova soba) (20% chance for each)
Igor je isto kao Noćni policajac, samo po danu (spawna se bilo di s podjednakom šansom)
Djevojčica se spawna samo u dječoj sobi (ChildrenRoom)
Vrtlar se pojavljuje samo u vrtu (Garden)
Mehaničar se pojavljuje u skladištu (Warehouse) (70% chance) + Bathroomu (30% chance)
Slikar se pojavljuje u samo u attelieru
Nosač se pojavljuje na portirni (Recepetion) (90% chance) + skladištu (Warehouse) (10% chance)
Tourguide se pojavljuje samo na portirni (reception)

#####################################################################################
ČA JOŠ MORAN:
- prazne sekcije za merge
- definirat sekcije unutar soba




