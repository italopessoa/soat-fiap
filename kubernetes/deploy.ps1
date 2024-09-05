# Set-Location -Path "C:\path\to\your\yaml\files"

#Start-Job -ScriptBlock { minikube start } | Wait-Job

kubectl apply -f secret-db.yaml
kubectl apply -f configmap-api.yaml
kubectl apply -f configmap-db.yaml
kubectl apply -f secret-mercadopago.yaml
kubectl apply -f pv-db.yaml
kubectl apply -f pv-seq.yaml
kubectl apply -f pvc-db.yaml
kubectl apply -f pvc-seq.yaml
kubectl apply -f svc-mysql.yaml
kubectl apply -f deployment-db.yaml
kubectl apply -f deployment-api.yaml
kubectl apply -f deployment-seq.yaml
kubectl apply -f hpa-api.yaml
kubectl apply -f svc-seq.yaml
