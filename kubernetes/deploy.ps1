# Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
# All rights reserved.
#
# This source code is licensed under the BSD-style license found in the
# LICENSE file in the root directory of this source tree.

# Set-Location -Path "C:\path\to\your\yaml\files"
kubectl apply -f secret-db.yaml
kubectl apply -f configmap-api.yaml
kubectl apply -f configmap-db.yaml
kubectl apply -f secret-mercadopago.yaml
kubectl apply -f pv-db.yaml
kubectl apply -f pv-seq.yaml
kubectl apply -f pvc-db.yaml
kubectl apply -f pvc-seq.yaml
kubectl apply -f svc-mysql.yaml
kubectl apply -f pod-mysql.yaml

kubectl apply -f deployment-api.yaml
kubectl apply -f svc-api.yaml
kubectl apply -f hpa-api.yaml

kubectl apply -f svc-seq.yaml
kubectl apply -f pod-seq.yaml
