# Azure Dev Spaces
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
## Configure AKS
``` az aks use-dev-spaces -n developersdays-euw-aks -g developersdays-euw-rg```
