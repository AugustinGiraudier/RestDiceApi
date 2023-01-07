# DiceLauncher

## Présentation

### Objectif

Dice Launcher est une application vouée à simuler des lancés de dés entièrement personnalisables.  
  
Du Monopoly au Yams en passant par des jeux méconnus, la seule limite est l'utilisation de dés.  En effet, il est possible de les personnaliser entièrement pour leur donner le nombre de face souhaité.

Pour l'instant, le projet ne contient que le modèle et la gestion de base de données, mais une application android sera développée par la suite.

### Features

- Créer des dés personnalisables (faces et nombre de faces)
- créer des parties avec un nombre de dé personnalisable
- simuler des lancés au sein des parties

### Technologies

- .Net 6
- Entity Framework Core 6
- SQLite

## Prérequis

Afin d'utiliser DiceLauncher dans les meilleures conditions, munissez vous de Visual Studio  

Visual Studio 2022 peut etre trouvé [ici](https://visualstudio.microsoft.com/fr/vs/)  
Visual Studio 2019 peut etre trouvé [ici](https://visualstudio.microsoft.com/fr/vs/older-downloads/)  

Noubliez pas d'installer les modules de développement .Net

## Pour commencer 

Afin d'effectuer les migrations : 
* placez vous dans le dossier suivant :  
```
DiceLauncher\Sources\EntitiesLib
```  
* Executez :  
```
dotnet ef migrations add migration_1    
dotnet ef database update --startup-project ..\Application\
```

Note:

Si vous n'avez pas installé correctement EntityFrameworkCore, il vous faudra peut-être utiliser également :  
```
dotnet tool install --global dotnet-ef
```

Musissez vous ensuite de Visual Studio afin d'exécuter Programme.cs dans le package Application afin d'observer un lancement de test fonctionnel.

Vous pouvez également visionner le contenu de la base de donnée 
```
\Sources\Application\DiceLauncher.db
```
à l'aide du logiciel DbBrowser for SQLite disponible [ici](https://sqlitebrowser.org/)

## Documentation

Notre documentation est consultable [ici](https://codefirst.iut.uca.fr/documentation/augustin.giraudier/doxygen/DiceLauncher/html/index.html)