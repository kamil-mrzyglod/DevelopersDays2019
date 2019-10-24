# kubectl demo
## Setting the context
```
kubectl config set-context $(kubectl config current-context) --namespace=kubectl-demo
```
## Show working services
```
kubectl get services
```
## Ensure it works
```
kubectl describe service developersdays-back-service
```
## Delete pod to get new logs
```
kubectl delete pod developersdays-front-deployment-7d868dc669-6wz8d
```
## Check logs from a pod
```
kubectl logs -f developersdays-front-deployment-7d868dc669-c6xxk
```
