<p align="center">
  <a href="https://skillicons.dev">
    <img src="https://skillicons.dev/icons?i=azure,react,cs,dotnet,postgres,githubactions" />
  </a>
</p>

<h1 align="center">AI-Powered Email Assistant with Semantic Kernel and NEON DB</h1>

## Introduction

This project showcases a modern email assistant that leverages the power of Azure's Semantic Kernel, Microsoft Graph API, and NEON serverless PostgreSQL to create an intelligent email drafting and management system. The application can generate email drafts based on natural language descriptions, send them via Microsoft Graph API, and maintain a searchable history of both drafts and sent emails using vector embeddings for similarity search.

## Technologies Used

- **Frontend**: React with modern UI components
- **Backend**: .NET with Semantic Kernel
- **Database**: NEON Serverless PostgreSQL for vector embeddings storage
- **Authentication**: Microsoft Graph API for email integration
- **AI Features**: Azure OpenAI for text generation and embeddings
- **Vector Search**: Similarity search functionality using PostgreSQL vector operations

## Features

- Natural language to email draft generation using Semantic Kernel
- Direct email sending through Microsoft Graph API integration
- Vector embedding storage of email content in NEON PostgreSQL
- Similarity search across historical emails and drafts
- Modern React UI with responsive design
- Secure authentication and authorization
- Scalable serverless database architecture

## Architecture

The system consists of three main components:

1. **React Frontend**
   - Modern UI for email composition and management
   - Integration with backend services
   - Real-time draft preview and editing

2. **Semantic Kernel Backend**
   - Email draft generation using AI models
   - Vector embedding creation for email content
   - Graph API integration for email sending
   - API endpoints for frontend communication

3. **NEON PostgreSQL Storage**
   - Serverless vector database for embeddings
   - Efficient similarity search capabilities
   - Scalable storage solution

## Setup and Deployment

### Prerequisites
- Azure subscription with OpenAI access
- NEON PostgreSQL database
- Microsoft 365 developer account
- Node.js and .NET SDK

### Configuration
1. Set up Azure OpenAI service
2. Configure NEON PostgreSQL database
3. Set up Microsoft Graph API permissions
4. Configure environment variables for both frontend and backend

### Environment Variables
```env
AZURE_OPENAI_ENDPOINT=
AZURE_OPENAI_KEY=
NEON_CONNECTION_STRING=
MICROSOFT_GRAPH_CLIENT_ID=
MICROSOFT_GRAPH_CLIENT_SECRET=
```

## Future Enhancements

- Template management for common email types
- Advanced email analytics and insights
- Multi-language support
- Batch email processing
- Custom email classification

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request. For major changes, please open an issue first to discuss what you would like to change.

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Acknowledgments

- Thanks to the Semantic Kernel team for the amazing framework
- NEON team for the serverless PostgreSQL service
- The Microsoft Graph API team for the comprehensive email integration capabilities
