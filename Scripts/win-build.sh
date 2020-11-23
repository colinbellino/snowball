#!/usr/bin/env bash

"C:\Program Files\Unity\Hub\Editor\2019.3.11f1\Editor\Unity.exe" \
    -batchmode \
    -logfile "C:\tmp\accuracy-build.txt" \
    -quit \
    -customBuildName "Accuracy" \
    -projectPath "D:\Dev\Games\accuracy" \
    -buildTarget "StandaloneWindows64" \
    -customBuildTarget "StandaloneWindows64" \
    -customBuildPath "D:\Dev\Games\accuracy\Builds\StandaloneWindows64\Accuracy.exe" \
    -executeMethod "UnityBuilderAction.Builder.BuildProject" \
    -versioning "Semantic" \
    -version "0.0.3"
