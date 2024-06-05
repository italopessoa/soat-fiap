# Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
# All rights reserved.
#
# This source code is licensed under the BSD-style license found in the
# LICENSE file in the root directory of this source tree.

#!/bin/bash

for i in {1..10000} ; do
    curl http://127.0.0.1:60055/api/Products
    sleep $1
done
