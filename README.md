# Content Compare Tool

This command line tool runs a comparison between two versions of the Smarter Balanced content for the Sample Items Website and API. It is currently configured to apply the past year's content patch to the older content. The tool then runs queries to find various discrepancies between the content packages. The following files are outputted:
- A "Matching items diff" that finds all items that are in both content packages, outputting only the ones with differences. 
- A "New items" file with items that are in the new content package, but not the old one. 
- A "Missing Sample Items Website (SIW) requirements" file which has all the items in the new content package that are missing any of the following:
  - Standard Publications
  - Grade 
  - Subject
  - Interaction Type
- A "Get items without scoring" file with all items in the new content package not containing scoring information (Rubric or Scoring options).
- A "Scoring info diff" that compares the number of rubrics, rubric entries, samples, and scoring options between the old and new content packages. Note that this diff does not compare the contents of these lists, however. 
- A "Missing publications" file that contains all items in the new content package without a publication.

## Usage
There are a few configuration settings that will likely need to be changed before you can run the comparison tool. These settings can be found in [appsettings.override.json](Source/ContentCompare/appsettings.override.json).

- `SbContent.ContentRootDirectory` should be the absolute path to the root direcory of the old content package 
- `AppSettings.ContentCompareDirectory` should be the absolute path to the root directory new content package
- `AppSettings.OutputDirectory` is the directory the files described above are placed in after running the tool. By default, this is `Source/OutputFiles/`

This project was built using Visual Studio, so it may be easiest to run the tool from there, but you can also compile it and run it wherever the .NET Core runtime is installed. 

Assuming your present working directory is the root of this repo, you can build with 
```
$ cd Source/ContentCompare/
$ dotnet build 
```
And then run with 
```
$ dotnet bin/Debug/netcoreapp2.0/ContentCompare.dll
```
When you are running the tool, **make sure your present working directory is `.../Source/ContentCompare`!** Otherwise, the relative paths in `appsettings.override.json` may not line up. 