                           APPLICATION POUR LA GESTION DE LA FLOTTE
Dans ce fichier nous allons donner les démmarches a suivre pour lancer l'application
Première partie: 
-lorsque vous télécharger le dépôt du code source sur le Github, vous ouvrez le dossier avec visualstudio.
-Avant de lancer l'application vous devez installer les packages nécessaire pour le bon fonctionnement du logiciel:
	comme package dans notre cas nous avons:
		.BouncyCastle.Cryptography
		.BouncyCastle.NetCore
		.itext.bouncy-castle-adapter
		.itext7.pdfhtml
		.AspNetCore.Identity.EntityFrameworkCore
		....................UI
		.EntityFrameworkCore
		....................SqlServer
		....................Tools
		.Extentions.Identity.Stores
		.Extentions.DependencyInjection
Deuxième partie:
Cette partie conserne la base donnée
-il faut faires les migrations pour crée la base de donnée avec les commandes:
      - add-migration nom_de_la_migration
      -update-database -verbose
Troixième partie:
-Après avoir installer les packages, vous pouvez lancez l'application
- lorsque l'application est démarrer, vous sereai invitez a vous connecter, les identifiants de connexin sont:
    .UserName: florent
    .password: Florent446.com
-Après la connexion, vous aurez accès a toutes les fonctionnalitées du logiciels


