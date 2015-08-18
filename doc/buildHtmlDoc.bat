@echo off
echo Attempting to compile Markdown to HTML With Pandoc. 
echo If Pandoc is missing, this will fail.
echo http://pandoc.org

@pandoc --table-of-contents --template=UserDoc_buttondown.template -f Markdown_phpextra -t html5 -o UserDoc.html  UserDoc.md

echo.
echo Done.

