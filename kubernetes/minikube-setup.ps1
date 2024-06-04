# Set-Location -Path "C:\path\to\your\yaml\files"
kubectl apply -f secret-db.yaml
kubectl apply -f configmap-api.yaml
kubectl apply -f configmap-db.yaml
kubectl apply -f pv-db.yaml
kubectl apply -f pvc-db.yaml
kubectl apply -f svc-mysql.yaml
kubectl apply -f pod-mysql.yaml

kubectl apply -f deployment-api.yaml
kubectl apply -f svc-api.yaml
kubectl apply -f api-hpa.yaml

kubectl apply -f svc-seq.yaml
kubectl apply -f pod-seq.yaml
