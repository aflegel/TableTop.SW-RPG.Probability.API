# TableTop SW RPG Probability API

## What is this repository for?

* An app to visualize the probabilities of rolling dice for SW RPG
* 1.0.0

## How do I get set up?

* Configure database connection strings to your server
  * MS Sql and PostgreSql are supported
* Ensure the MyGet source is configured
* Run `nuget restore`
* Once the package restore is complete the databases should create themselves with the proper model
  * To populate the statistics database you'll need to run the data generator project.

## Contribution guidelines

* [Learn Markdown](https://bitbucket.org/tutorials/markdowndemo)

### C# Standards

* Tabs over spaces