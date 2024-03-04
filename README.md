Typical live chat app to play around with SignalR. I widened it to practice from scratch:

- Docker: First thing I did after checking the app worked was dockerizing it.
- Terraform: I use Terraform to manage the cloud services needed to deploy it
- Azure containers group and Azure App Services: I used both services, just for fun. App Services has a free tier and Container Group is quite stright forward but costly.
- Azure Devops: any change in Gitbuh triggers an Azure Devops pipeline that runs terraform to create cloud services, create a docker container, push the container to docker hub and deploy that container into Azure.
- ChatGPT: added ChatGPT API just for fun.
