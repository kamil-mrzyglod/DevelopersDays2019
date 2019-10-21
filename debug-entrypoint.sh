#!/bin/bash

echo "D: Adding symlinks for telepresence mounts"

IFS=':' read -ra TELEPRESENCE_MOUNTS_ARRAY <<< $TELEPRESENCE_MOUNTS
for mount in "${TELEPRESENCE_MOUNTS_ARRAY[@]}"; do
    mkdir -p $(dirname $mount)
    ln -s $TELEPRESENCE_ROOT/$mount $(dirname $mount)
done

echo "D: Everything ready - don’t fix bugs later; fix them now."

tail -f /dev/null