#!/bin/bash

# Make sure globstar is enabled
shopt -s globstar
resizeTo="256"

for image_file in content/**/*.png
do


#nameFront="${image_file#*/}"
nameFront="${image_file%% *}"
resizingCommand="${image_file}[${resizeTo}x${resizeTo}]"
outputFilename="${nameFront} ${resizeTo}.png"

echo "${resizingCommand} => ${outputFilename}"

magick "${resizingCommand}" "${outputFilename}"

done