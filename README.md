<p align="center">
  <a href="https://skillicons.dev">
    <img src="https://skillicons.dev/icons?i=azure,vscode,cs,react,tailwind,githubactions,postgres" />
  </a>
</p>

<h1 align="center">Semantic Kernel-Email Assistant with NEON Serverless memory store</h1>

## Introduction

Today, we’ll explore how to integrate Azure AI Containers into our applications running on Azure Kubernetes Service (AKS). Azure AI Containers enable you to harness the power of Azure’s AI services directly within your AKS environment, giving you complete control over where your data is processed. By streamlining the deployment process and ensuring consistency, Azure AI Containers simplify the integration of cutting-edge AI capabilities into your applications. Whether you’re developing tools for education, enhancing accessibility, or creating innovative user experiences, this guide will show you how to seamlessly incorporate Azure’s AI Containers into your web apps running on AKS.

**Azure AI Containers**

Azure AI services provides several Docker containers that let you use the same APIs that are available in Azure, on-premises. Using these containers gives you the flexibility to bring Azure AI services closer to your data for compliance, security or other operational reasons. Container support is currently available for a subset of Azure AI services.

Azure AI Containers offer:

   - Immutable infrastructure: Consistent and reliable system parameters for DevOps teams, with flexibility to adapt and avoid configuration drift.
   - Data control: Choose where data is processed, essential for data residency or security requirements.
   - Model update control: Flexibility in versioning and updating deployed models.
   - Portable architecture: Deploy on Azure, on-premises, or at the edge, with Kubernetes support.
   - High throughput/low latency: Scale for demanding workloads by running Azure AI services close to data and logic.
   - Scalability: Built on scalable cluster technology like Kubernetes for high availability and adaptable performance

## Project Build

### Frontend

- **Python FLlask Web UI**: The frontend of our system, a simple Text Input and "Submit" button

### Backend

- **API Endpoint**: The backend is hosted into AKS with the Sentiment Analysis Container and communicates with a Language AI Resource for billing and metrics.


### Deployment and Hosting

- **Docker Containers**: Both the frontend and backend components are containerized, the API Backend is called directly via mcr.microsoft.com and the Frontend is pushed in Azure Container Registry.
- **Azure Container Registry**: The Docker image is pushed to Azure Container Registry, from where they are managed and deployed.

## Features

- Scalable Environment of AKS to handle various trafiic needs.
- Simple UI build in Python.
- Secure backend API within the AKS Cluster.
- Docker containerization for consistent deployment and scalability.
- Integration with Azure services for robust and reliable application performance.
- Latest Sentiment Analysis Image for precise analysis.

## Conclusion

By leveraging these technologies, you gain granular control over your data and model deployments, while maintaining the scalability and portability essential for modern applications. Remember, this is just the starting point. As you delve deeper, consider the specific requirements of your project and explore the vast possibilities that Azure AI Containers unlock. Embrace the power of AI within your AKS deployments, and you’ll be well on your way to building innovative, intelligent solutions that redefine what’s possible in the cloud.
## Instructions
**Follow the Blog for Detailed Instructions**: For step-by-step guidance, visit [Azure AI Services on AKS](https://www.cloudblogger.eu/2024/05/17/azure-ai-cloud-native-on-aks/).

## Contribution

Contributions are welcome! If you have suggestions or improvements, feel free to fork the repository, make your changes, and submit a pull request.

## Architecture
![architect-aiaks-1](https://github.com/passadis/aks-aicontainers/assets/53148138/eb1b0e05-5d61-4e91-b9a6-7b751429b4b2)
