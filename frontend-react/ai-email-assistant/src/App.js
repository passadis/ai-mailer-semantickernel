import React, { useState } from 'react';
import logo from './logo1.png'; // Import the logo image file
import './styles.css'; // Import the CSS file

function App() {
  const [email, setEmail] = useState('');
  const [subject, setSubject] = useState('');
  const [description, setDescription] = useState('');
  const [draft, setDraft] = useState('');
  const [status, setStatus] = useState('');

  // URLs from React environment variables
  const backendBaseUrl = process.env.REACT_APP_API_URL;

  const handleGenerateDraft = async () => {
    try {
      const response = await fetch(`${backendBaseUrl}/email/generate-email`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          subject,
          description,
        }),
      });

      if (!response.ok) {
        throw new Error('Failed to generate draft');
      }

      const data = await response.json();
      setDraft(data.draft);
      setStatus('');
    } catch (error) {
      console.error('Error generating draft:', error);
      setStatus('Failed to generate draft.');
    }
  };

  const handleSendEmail = async () => {
    try {
      const response = await fetch(`${backendBaseUrl}/email/send`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          subject,
          draft,
          recipientEmail: email,
        }),
      });

      if (!response.ok) {
        throw new Error('Failed to send email');
      }

      setStatus('Email sent successfully!');
    } catch (error) {
      console.error('Error sending email:', error);
      setStatus('Failed to send email.');
    }
  };

  return (
    <div className="container">
      {/* Logo */}
      <img src={logo} alt="Logo" className="logo" />

      {/* Title */}
      <h1>AI Email Assistant</h1>

      {/* Input Form */}
      <label>Recipient Email:</label>
      <input
        type="email"
        value={email}
        onChange={(e) => setEmail(e.target.value)}
        placeholder="Enter recipient's email"
      />

      <label>Subject:</label>
      <input
        type="text"
        value={subject}
        onChange={(e) => setSubject(e.target.value)}
        placeholder="Enter email subject"
      />

      <label>Description:</label>
      <textarea
        value={description}
        onChange={(e) => setDescription(e.target.value)}
        placeholder="Enter a brief description of the email"
      />

      <button onClick={handleGenerateDraft}>Generate Draft</button>

      {/* AI Generated Draft */}
      {draft && (
        <div className="chat-display">
          <h2>AI Draft:</h2>
          <textarea
            className="editable-draft"
            value={draft}
            onChange={(e) => setDraft(e.target.value)}
          />
          <button onClick={handleSendEmail}>Approve and Send</button>
        </div>
      )}

      {/* Status Display */}
      {status && (
        <div
          className={`status ${
            status.includes('successfully') ? 'success' : 'error'
          }`}
        >
          {status}
        </div>
      )}
    </div>
  );
}

export default App;
