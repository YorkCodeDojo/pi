cmake_minimum_required(VERSION 3.20)
project(pi LANGUAGES CXX)

set(CMAKE_CXX_STANDARD 26)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

if(CMAKE_CXX_COMPILER_ID MATCHES "Clang")
  add_compile_options(-stdlib=libc++)
  add_link_options(-stdlib=libc++)
endif()

add_executable(pi)
target_sources(
        pi
        PRIVATE
        main.cpp
)
