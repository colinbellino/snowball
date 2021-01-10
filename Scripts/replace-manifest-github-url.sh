#!/bin/bash

# Replaces github urls with ones with token so we can clone private repositories:
# https://github.com/ -> https://{PRIVATE_TOKEN}:@github.com/

cp ./Packages/manifest.json ./Packages/manifest.original.json

original="https://github.com/"
replacement="https://$PRIVATE_TOKEN:@github.com/"
sed -i 's,'"$original"','"$replacement"',g' ./Packages/manifest.json
