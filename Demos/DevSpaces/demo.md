# Azure Dev Spaces
## Setting the context
```
kubectl config set-context $(kubectl config current-context) --namespace=default
```
## Configure AKS
``` 
az aks use-dev-spaces -n developersdays-euw-aks -g developersdays-euw-rg
```
## Namespaces with Azure Dev Spaces enabled
```
kubectl get namespaces
kubectl get deployments -n azds
kubectl get services -n azds
kubectl get pods -n azds
```
## Project preparation
```azds prep --public```
## Run project
```azds up```
## Running containers
```
kubectl get pods
kubectl logs developersdaysback-dfb85845d-264zq
```