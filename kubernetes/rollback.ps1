# Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
# All rights reserved.
#
# This source code is licensed under the BSD-style license found in the
# LICENSE file in the root directory of this source tree.

# Set-Location -Path "C:\path\to\your\yaml\files"
kubectl delete -f hpa-api.yaml
kubectl delete -f deployment-api.yaml
kubectl delete -f svc-api.yaml

kubectl delete -f deployment-db.yaml
kubectl delete -f svc-mysql.yaml

kubectl delete -f secret-mercadopago.yaml
kubectl delete -f secret-db.yaml

kubectl delete -f configmap-db.yaml
kubectl delete -f configmap-api.yaml

kubectl delete -f svc-seq.yaml
kubectl delete -f deployment-seq.yaml

kubectl delete -f pvc-db.yaml
kubectl delete -f pv-db.yaml

kubectl delete -f deployment-seq.yaml
kubectl delete -f pvc-seq.yaml
kubectl delete -f pv-seq.yaml
