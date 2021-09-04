minikube start --driver=docker --ports=30080:30080

kubectl apply -f platforms-deployment.yml  
kubectl apply -f platforms-nodeport-service.yml

kubectl get deployments    
kubectl get pods    
kubectl get services