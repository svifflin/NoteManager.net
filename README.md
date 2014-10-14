NoteManager.net
===============

Un note manager utilisant les technologies .Net et qui nécessite le framework 4.0 au minimum


Spécifications fonctionnelles
===============

L'application permet la prise de notes facilement, et le stockage sur un serveur distant. Ces notes ont un titre et sont stockées avec la date et l'heure de création. Un champ de recherche textuelle permet de retrouver une note en recherchant dans les titres. Le résultat de recherche est présenté dans l'ordre chronologique inverse, du plus récent au plus ancien. Il est possible de supprimer une note.

En résumé :

* Créer / éditer / supprimer une note
* Une note est composée d'un titre, d'un texte comprenant la note elle-même, d'une date de création et de dernière modification.
* Les données sont stockées sur un serveur de données distant.
* Recherche textuelle d'une note en cherchant dans les titres, optionnellement dans le texte aussi.
* Le résultat apparaît dans l'ordre chronologique inverse, du plus récent au plus ancien, en prenant en compte la date de modification. L'ordre peut être inversé.
* L'application peut être utilisée par différents utilisateurs.
* Un web service WCF permet d'accéder aux données, rendant ainsi disponible l'application pour Silverlight, Windows Phone, ....


Spécifications techniques
===============
L'interface est réalisée en WPF, et utilise le design pattern MVVM. (il n'y a pas de code dans le codebehind des vues).
La gestion des données est réalisée avec une base données SQL en utilisant Entity Framework et MSSQL.


Base de données
===============

Le script d'installation de la base de données se trouve dans le dossier NoteManager.Data/NoteManagerDataModel.edmx.sql
