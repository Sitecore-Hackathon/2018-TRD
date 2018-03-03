# Sitecore Similar Image Search
## Installation Manual

1. You will need to have SXA 1.5 or 1.6 Installed on your Sitecore instance
2. Install Sitecore Similar Image Search (SIS) package which includes DLLs, configs, few templates, a rendering and a settings item
3. Go to /sitecore/System/Settings/Feature/Similar Image Search/Indexing Settings and updated the 'Indexing Scope' field to the folder you want the crawler to crawler its content for images.
4. Go to your SXA site, under /Presentation/Available Renderings/Search, include the new rendering (/sitecore/Layout/Renderings/Feature/Similar Image Search/Search Results) into the Renderings field.
5. Duplicate your current instance's Sitecore_Master_Index core, delete the data folder and rename the folder and the name inside the core.properties into 'sch1_sis_master_index'
6. Inside the core's config folder, open managed-schema and add a new field:
```
<field name="sis_dom_table" type="string" indexed="true" stored="true"/>
```
7. Save the and restart your Solr service
8. Go back to Sitecore and rebuild the new Sitecore_sis_master_index
9. Open the Experience Editor (SXA), add the 'Search Results' component from the new 'Similar Search Results' section
10. You can now upload an image and get similar images by color dominance