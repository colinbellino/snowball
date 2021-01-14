#!/usr/bin/env bash

"C:\Program Files\Unity\Hub\Editor\2020.2.1f1\Editor\Unity.exe" \
    -batchmode \
    -logfile "C:\tmp\snowball-build.txt" \
    -quit \
    -customBuildName "snowball" \
    -projectPath "D:\Dev\Games\snowball" \
    -buildTarget "StandaloneWindows64" \
    -customBuildTarget "StandaloneWindows64" \
    -customBuildPath "D:\Dev\Games\snowball\Builds\StandaloneWindows64\snowball.exe" \
    -executeMethod "UnityBuilderAction.Builder.BuildProject" \
    -versioning "Semantic" \
    -version "0.0.3"
