cmake_minimum_required(VERSION 3.16)

set(AX_ROOT "/Users/ooharayukio/ext/axmol")
list(APPEND CMAKE_MODULE_PATH "${AX_ROOT}/cmake/Modules")

include(AxmolBuildHelpers)

project(AxmolTsumTsumu)

add_subdirectory("${AX_ROOT}/core" ${CMAKE_BINARY_DIR}/axmol/core)

set(GAME_SOURCES
    Source/AppDelegate.h
    Source/AppDelegate.cpp
    Source/MainScene.h
    Source/MainScene.cpp
    Source/Ball.h
    Source/Ball.cpp
)

setup_axmol_app(AxmolTsumTsumu SOURCES ${GAME_SOURCES})
