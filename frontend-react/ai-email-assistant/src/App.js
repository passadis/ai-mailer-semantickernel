import React, { useState, useEffect } from 'react';
import logo from './logo1.png';
import './styles.css';
import PinAuth from './Components/PinAuth'; // Import the PinAuth component

function App() {
  const [email, setEmail] = useState('');
  const [subject, setSubject] = useState('');
  const [description, setDescription] = useState('');
  const [draft, setDraft] = useState('');
  const [status, setStatus] = useState('');
  const [loading, setLoading] = useState(false);
  const [searchInput, setSearchInput] = useState('');
  const [searchResults, setSearchResults] = useState([]);

  const backendBaseUrl = process.env.REACT_APP_API_URL;

  const handleGenerateDraft = async () => {
    setLoading(true); // Start loading
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
      setStatus(''); // Clear any previous status
    } catch (error) {
      console.error('Error generating draft:', error);
      setStatus('Failed to generate draft.');
    } finally {
      setLoading(false); // End loading
    }
  };

  const handleSendEmail = async () => {
    setLoading(true);
    try {
      const payload = {
        subject,
        body: draft.replace(/\n/g, '<br>'),
        recipients: [email],
      };

      const response = await fetch(`${backendBaseUrl}/email/send`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(payload),
      });

      if (!response.ok) {
        throw new Error('Failed to send email');
      }

      setStatus('Email sent successfully!');
    } catch (error) {
      console.error('Error sending email:', error);
      setStatus('Failed to send email.');
    } finally {
      setLoading(false);
    }
  };

  const handleSearch = async () => {
    try {
      const response = await fetch(`${backendBaseUrl}/embedding/search`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ input: searchInput, limit: 5 }),
      });

      if (!response.ok) {
        throw new Error('Failed to retrieve search results');
      }

      const data = await response.json();
      setSearchResults(data);
    } catch (error) {
      console.error('Error fetching search results:', error);
    }
  };

  // Automatically clear status messages after 5 seconds
  useEffect(() => {
    if (status) {
      const timeout = setTimeout(() => setStatus(''), 5000);
      return () => clearTimeout(timeout);
    }
  }, [status]);

  return (
    <PinAuth>
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

      {loading ? (
        <div className="loading-spinner">
          <div className="spinner"></div>
          <p>Processing, please wait...</p>
        </div>
      ) : (
        <button onClick={handleGenerateDraft}>Generate Draft</button>
      )}

      {/* AI Generated Draft */}
      {draft && (
        <div className="chat-display">
          <h2>AI Draft:</h2>
          <textarea
            className="editable-draft"
            value={draft}
            onChange={(e) => setDraft(e.target.value)}
          />
          <button onClick={handleSendEmail} disabled={loading}>
            Approve and Send
          </button>
        </div>
      )}

      {/* Search Form */}
      <label>Search for Similar Text:</label>
      <input
        type="text"
        value={searchInput}
        onChange={(e) => setSearchInput(e.target.value)}
        placeholder="Enter text to search for similar entries"
      />
      <button onClick={handleSearch}>Search</button>

      {/* Search Results */}
      {searchResults.length > 0 && (
        <div className="search-results">
          <h2>Search Results:</h2>
          <ul>
            {searchResults.map((result, index) => (
              <li key={index}>
                <strong>Text:</strong> {result.inputText} <br />
                <strong>Similarity:</strong> {result.similarity.toFixed(2)}
              </li>
            ))}
          </ul>
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
    </PinAuth>
  );
}

export default App;
