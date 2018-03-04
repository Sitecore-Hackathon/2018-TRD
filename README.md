# Sitecore Similar Image Search
## Team TRD
### Sitecore Hackathon 2018
### Hackathon Category: Sitecore Experience Accelerator (SXA)

### Module Purpose:
Images these days are everywhere. What if your client wanted users to search your Media Library for images and be able to start your search using a sample image that users provide.
This Sitecore SXA module allows Sitecore users to simply add the component to their pages and right away allow public users to start search for similar images from Sitecore Media Library.

### How does the end user use the Module?
Sitecore users will simply drag and drop the new Search Results (Similar Image Search) component into the page and set the 'Search Scope' properties using the Component Properties.
![SXA ToolBox](documentation/images/Toolbox.PNG?raw=true "SXA ToolBox")
![Upload](documentation/images/upload.PNG?raw=true "Upload")
![Results](documentation/images/SXAOutput.PNG?raw=true "Results")
'sitecore_sis_master_index' index should already by run after specifying the indexing scope in the Settings item /sitecore/Settings/feature/Similar Image Search/Indexing Settings.
![Settings](documentation/images/Settings.PNG?raw=true "Settings")
Indexing will get all the image media items and extract meaningfull data about the dominant color of each quadrant of the image and store them into Solr. Indexing will time quite some time since some processing will occur on each image.

### Video:
https://youtu.be/G-ZeS3NaPUo

#### The module uses Color Dominance algorithm which is not very accurate but can help get images with similar color themes
#### To install please see documentation/README.md
### What Next:
Due to the nature of Solr sorting, it didn't support retrieving images directly from Solr without having to cache them all in memory. However, we may be able to create custom java functions defined in Solr that hopefully could be used to get similar images.

Adding custom parameters and variants to allow more flexible customizations and usage
