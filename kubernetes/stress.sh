#!/bin/bash

for i in {1..10000} ; do
    curl http://127.0.0.1:60055/api/Products
    sleep $1
done
