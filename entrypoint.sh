# configure k8s
mkdir ~/.kube

if [ -f "/kube/admin.conf" ]; then 
    cp /kube/admin.conf ~/.kube/config
else
    cp /kube/config ~/.kube
fi

exec telepresence "$@"