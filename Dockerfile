FROM debian:buster-slim

RUN apt-get update \
 && apt-get install -y --no-install-recommends curl \
 && apt-get install -y --no-install-recommends gnupg net-tools

RUN apt-get update \
 && apt-get install -y --no-install-recommends apt-transport-https ca-certificates software-properties-common

# Install docker cli
ENV DOCKERVERSION=19.03.4
RUN curl -fsSLO https://download.docker.com/linux/static/stable/x86_64/docker-${DOCKERVERSION}.tgz \
  && tar xzvf docker-${DOCKERVERSION}.tgz --strip 1 -C /usr/local/bin docker/docker \
  && rm docker-${DOCKERVERSION}.tgz

# Install telepresence
RUN curl -s https://packagecloud.io/install/repositories/datawireio/telepresence/script.deb.sh | bash && apt install --no-install-recommends -y telepresence

## Install kubectl
RUN curl -s https://packages.cloud.google.com/apt/doc/apt-key.gpg | apt-key add -
RUN touch /etc/apt/sources.list.d/kubernetes.list
RUN echo "deb http://apt.kubernetes.io/ kubernetes-xenial main" | tee -a /etc/apt/sources.list.d/kubernetes.list

RUN apt-get update \
 && apt-get -y --no-install-recommends install kubectl

ENTRYPOINT ["telepresence"]