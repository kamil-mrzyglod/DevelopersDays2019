FROM ubuntu:18.04
RUN apt-get update && \
      apt-get -y install sudo
RUN useradd -m docker && echo "docker:docker" | chpasswd && adduser docker sudo

RUN apt-get update && apt-get install -y curl && apt-get install -y gnupg net-tools

RUN apt-get update && \
	apt-get install \
   -y apt-transport-https \
    ca-certificates \
    software-properties-common

# Install docker
RUN curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo apt-key add - && \
		apt-key fingerprint 0EBFCD88 && \
		add-apt-repository \
			"deb [arch=amd64] https://download.docker.com/linux/ubuntu \
			$(lsb_release -cs) \
		stable"	&&	apt-get update && apt-get install -y docker-ce

# Install telepresence
RUN curl -s https://packagecloud.io/install/repositories/datawireio/telepresence/script.deb.sh | bash && apt install --no-install-recommends -y telepresence

## Install kubectl
RUN curl -s https://packages.cloud.google.com/apt/doc/apt-key.gpg | apt-key add -
RUN touch /etc/apt/sources.list.d/kubernetes.list
RUN echo "deb http://apt.kubernetes.io/ kubernetes-xenial main" | tee -a /etc/apt/sources.list.d/kubernetes.list

RUN apt-get update && apt-get -y install kubectl

COPY ./entrypoint.sh .
ENTRYPOINT ["/bin/bash", "./entrypoint.sh"]