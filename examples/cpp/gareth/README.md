```bash
CC=clang-20 CXX=clang++-20 \
cmake \
  -G Ninja \
  -B build \
  -S . \
  -D CMAKE_BUILD_TYPE=Release

cmake --build build

./build/pi
```
