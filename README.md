# Jeu-de-cartes-SSI
Jeu de cartes SSI, Sensibilisation à la sécurité des systèmes d’information

# Pour télécharger le jeu, rendez vous sur le site : 
- https://github.com/SecuriteSystemesInformations/Jeu-de-cartes-SSI/

# Sources
Ordinateurs :
- https://www.cgtrader.com/items/836625/download-page#

Personnages:
- https://www.mixamo.com/#/?limit=96&page=1&type=Character

Table de billard : 
- https://3dsky.org/3dmodels/show/bil_iardnyi_stol_14
- https://3dsky.org/3dmodels/show/bil_iardnyi_stol_wik_kantslier

Chaise :
- https://www.turbosquid.com/3d-models/armchair-chair-3ds-free/832916

Interface de connexion :
- https://assetstore.unity.com/packages/essentials/network-lobby-41836

Cheval de Troie (icône Trojan) :
- https://www.kisscc0.com/clipart/malware-trojan-horse-computer-software-computer-vi-xp9mwl/

Access denied
- https://commons.wikimedia.org/wiki/File:Access-denied_story.jpg
- By Adelson Raimundo Reis Amaral [CC BY-SA 4.0  (https://creativecommons.org/licenses/by-sa/4.0)], from Wikimedia Commons

Access granted :
- By Adelson Raimundo Reis Amaral [CC BY-SA 4.0  (https://creativecommons.org/licenses/by-sa/4.0)], from Wikimedia Commons

Rideau :
- https://www.turbosquid.com/FullPreview/Index.cfm/ID/1034276

Machine à sous :
- https://www.turbosquid.com/3d-models/3d-arcade-machine-1212120

![Image du projet](https://drive.google.com/uc?export=view&id=10cGfM-noIL_E6wjTzp1_dZdvDWGN4OqZ)

# Sommaire
- [Avant de commencer](#avant-de-commencer)
- [Quelques conventions de nommages](#quelques-conventions-de-nommages)
- [Creation de branches pour les evolutions](#creation-de-branches-pour-les-evolutions)
- [Importer le projet git dans Eclipse](#importer-le-projet-git-dans-eclipse)
- [Changer de branche git dans Eclipse](#changer-de-branche-git-dans-eclipse)
- [Mettre a jour son repertoire git local avec le repertoire distant](#mettre-a-jour-son-repertoire-git-local-avec-le-repertoire-distant)
- [Fusionner les modifications faites sur une autre branche avec la branche courante](#fusionner-les-modifications-faites-sur-une-autre-branche-avec-la-branche-courante)
- [Si des erreurs apparaissent](#si-des-erreurs-apparaissent)

# Avant de commencer
- Dans certains dossiers, vous trouverez un fichier oneFileIsRequired.txt. Vous pourrez supprimer ces fichiers lorsque le dossier contiendra au moins un autre fichier (par exemple un fichier .java). Si vous supprimez ce fichier .txt et que le dossier devient vide, alors le dossier ne pourra pas être 'commit' ce qui pourra entraîner des erreurs de compilation.

# Quelques conventions de nommages
- Le nom des variables ainsi que le nom des fonctions sont en anglais
- Le nom des interfaces java débutera toujours par un 'I'

# Creation de branches pour les evolutions
1. Cliquez sur le bouton + (situé à côté du nom de la branche courante)
2. Cliquez sur 'New branch'
3. Vous serez redirigez vers une fenêtre qui vous demandera d'indiquer le nom de votre nouvelle branche. Ce nom doit être le nom de l'évolution de manière succinte et les mots seront séparés par des underscores. Vous devrez également sélectionner la branche  partir de laquelle vous voulez créer la branche : vérifiez que c'est bien la branche master (ou une autre branche si elle est bien à jour)
4. Puis pour travailler sur cette branche dans Eclipse, suivez mettez à jour le projet(#mettre-a-jour-son-repertoire-git-local-avec-le-repertoire-distant) et changez de branche(#changer-de-branche-git-dans-eclipse).

# Importer le projet git dans Eclipse
1. Copiez le lien suivant : https://gitlab.com/climg/monitrack.git (Ce lien apparait lorsque vous cliquez sur le bouton clone en haut à droite de la page puis dans la partie 'Clone with HTTPS')
2. Rendez-vous dans Eclipse
3. Cliquez sur Window > Perspective > Open Perspective > Git
4. Cliquez sur Clone (soit le petit nuage avec une flèche verte ou alors un lien qui s'affiche)
5. Dans l'URI, collez le lien que vous avez copier précédemment.
6. Renseignez votre user (prenom.nom) et votre password.
7. Sélectionnez toutes les branches, puis faites 'Next'.
8. Choisissez l'emplacement du projet, puis faites'Finish'.
9. Une fois que le projet apparaît dans la fenêtre 'Git Repositories', faites une clique-droit sur le projet puis 'Import projects'

# Changer de branche git dans Eclipse
1. Assurez-vous d'avoir mis à jour votre répertoire avec le répertoire distant(#mettre-a-jour-son-repertoire-git-local-avec-le-repertoire-distant)
2. Dans 'Project Explorer', faites un clic-droit sur 'monitrack', puis Team > Switch To > Other

	- Si vous n'avez pas utilisé la branche dans le projet :
		-> cherchez dans le dossier 'Local'	et la sélectionner
	- Si vous avez déjà utilisé la branche : 
		-> cherchez dans le dossier 'Remote Tracking'
		-> Sélectionnez la branche puis cliquez sur 'Checkout as a new Local Branch'
3. Faites un clic-droit sur monitrack > Maven > Update Project
4. Puis clic-droit sur monitrack > Run As > Maven Clean
5. Puis clic-droit sur monitrack > Run As > Maven Install
6. Vous pouvez ensuite travailler sur cette branche

# Mettre a jour son repertoire git local avec le repertoire distant
1. Dans 'Project Explorer', faites un clic-droit sur 'monitrack', puis Team > Fetch from Upstream
2. Puis clic-droit sur 'monitrack', Team > Pull

# Fusionner les modifications faites sur une autre branche avec la branche courante
1. Assurez-vous d'avoir mis à jour votre répertoire avec le répertoire distant(#mettre-a-jour-son-repertoire-git-local-avec-le-repertoire-distant)
2. Dans 'Project Explorer', faites un clic-droit sur 'monitrack', puis Team > Fetch from Upstream

# Si des erreurs apparaissent
- En cas de problèmes lors de l'exportation du projet Maven en fichier .jar, vous pouvez consulter ce site(#https://www.codejava.net/coding/how-to-create-executable-jar-file-with-resources-and-dependencies-using-maven-in-eclipse)

"Error when trying to fetch or push" (Source : https://stackoverflow.com/questions/22824170/eclipse-egit-error-when-trying-to-fetch-or-push) 
- Clique-droit sur le projet -> Team -> Remote -> Configure push to upstream->URI, Change-> Add authentication details
