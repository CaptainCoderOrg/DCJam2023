#!/bin/bash
dotnet build CaptainCoder.Core/ -c Release
CORE_PATH="CaptainCoder.Core/CaptainCoder/Core/bin/Release/netstandard2.1"
CORE="CaptainCoder.Core"
CORE_UNITY_PATH="CaptainCoder.Core/CaptainCoder/Core.UnityEngine/bin/Release/netstandard2.1"
CORE_UNITY="CaptainCoder.Core.UnityEngine"
DUNGEONEERING_PATH="CaptainCoder.Core/CaptainCoder/Dungeoneering/bin/Release/netstandard2.1"
DUNGEONEERING="CaptainCoder.Dungeoneering"
UNITY_DLL_PATH="Duality Dungeon Crawler/Assets/Plugins/CaptainCoder"
cp "$CORE_PATH/$CORE.dll" \
    "$CORE_PATH/$CORE.xml" \
    "$CORE_UNITY_PATH/$CORE_UNITY.dll" \
    "$CORE_UNITY_PATH/$CORE_UNITY.xml" \
    "$DUNGEONEERING_PATH/$DUNGEONEERING.dll" \
    "$DUNGEONEERING_PATH/$DUNGEONEERING.xml" \
    "$UNITY_DLL_PATH/"