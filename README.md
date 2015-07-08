## Welcome to the DotNET.PhotoWF wiki!

In the last couple of years my personal photographs were organized the following way:

* **Step#1** Downloading images into a folder  which is labelled by date and some keywords

   i.e.: "C:\Photo\2015-03-21 Zoe cycling" 

![](https://dl.dropboxusercontent.com/u/16561369/Static/GitHub/Repos/DotNET.PhotoWF/Photos_Organized.JPG)

* **Step#2** Later when all the event related pictures are gathered the family sits down and we check the photos one-by-one looking for the best shots - a subfolder called "best" is created

e.g: "C:\Photo\2015-03-21 Zoe cycling\best" subfolder gets created and the selected images are copied over there

![](https://dl.dropboxusercontent.com/u/16561369/Static/GitHub/Repos/DotNET.PhotoWF/Photos_Organized_OK.jpg)

Supporting this work-flow two command line tools were created:

* **PhotoWFDirReport** - which can look for each folder that were not processed yet (Step#2):

![](https://dl.dropboxusercontent.com/u/16561369/Static/GitHub/Repos/DotNET.PhotoWF/Photos_Organized_NotOK.jpg)

![](https://dl.dropboxusercontent.com/u/16561369/Static/GitHub/Repos/DotNET.PhotoWF/PhotoWFDirReport_m_switch.jpg)

* **PhotoWFExifProc**  - is able to enrich the image files with EXIF tags based on the folder structure. 

i.e.: JPG files in the "C:\Photo\2015-03-21 Zoe cycling\best" folder get enriched with the exif tags: "Zoe" "Cycling" "Best" "2015-03-21" 
	
![](https://dl.dropboxusercontent.com/u/16561369/Static/GitHub/Repos/DotNET.PhotoWF/PhotoWFExifProc_f_switch.JPG)

![](https://dl.dropboxusercontent.com/u/16561369/Static/GitHub/Repos/DotNET.PhotoWF/PhotoWFExifProc_result_tags%20populated.JPG)
					
or prints all the EXIF tags found beneath a folder:

![](https://dl.dropboxusercontent.com/u/16561369/Static/GitHub/Repos/DotNET.PhotoWF/PhotoWFExifProc_g_switch.JPG)
