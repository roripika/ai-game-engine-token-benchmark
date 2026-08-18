cmake_minimum_required(VERSION 3.10)

project(AxmolTsumTsumu)

set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

# Axmol Engine 参照またはスタンドアロンソース構成
file(GLOB_RECURSE GAME_SOURCES "Source/*.cpp" "Source/*.h")

add_executable(${PROJECT_NAME} ${GAME_SOURCES})

# Include directories
target_include_directories(${PROJECT_NAME} PUBLIC Source)
