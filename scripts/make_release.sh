#!/bin/sh -eu

if [ $# -ne 2 ]; then
  echo $#
  echo "$0: syntax: <version> <internals path>" >&2
  exit 1
fi

rm -fr tmp/
mkdir tmp/

header="// Author: delthas
// Date: $(date +%F)
// License: MIT
// Source: https://github.com/delthas/vegas-datamosh
// Documentation: https://github.com/delthas/vegas-datamosh
// Version: $1
//
"

for f in *.cs; do
  echo -n "$header" | cat - "$f" > "tmp/$f"
  sed -r 's/using Sony\.Vegas/using ScriptPortal\.Vegas/g' "tmp/$f" > "tmp/${f%.*}14.cs"
  cp "$f.config" "tmp/$f.config"
  cp "$f.config" "tmp/${f%.*}14.cs.config"
done

cp LICENSE README.md *.png tmp/
cp -R "$2" tmp/_internal/

for f in _internal/*.js; do
  tail -n +2 "$f" > "tmp/$f.tmp"
  echo -en "//AD\n$header" | cat - "tmp/$f.tmp" > "tmp/$f"
  rm "tmp/$f.tmp"
done

rm windows64.zip
cd tmp
zip -qr9 ../windows64.zip *
cd ..
rm -fr tmp/

echo "release $1 created successfully: windows64.zip"
