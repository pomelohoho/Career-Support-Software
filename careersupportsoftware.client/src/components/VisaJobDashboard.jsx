
// src/components/VisaJobDashboard.jsx
import React, { useState, useEffect } from "react";
import PropTypes from 'prop-types';
import { createTheme, ThemeProvider } from "@mui/material/styles";
import {
  Box,
  Grid,
  Card,
  CardContent,
  Divider,
  List,
  ListItem,
  ListItemIcon,
  ListItemText,
  Avatar,
  Chip,
  TextField,
  InputAdornment,
  Typography,
  CircularProgress,
  Alert,
  Pagination,
} from "@mui/material";
import {
  Work as WorkIcon,
  Notifications,
  Settings,
  Search as SearchIcon
} from "@mui/icons-material";

import Header from "./Header";


// theme setup 
const lightTheme = createTheme({
  palette: {
    mode: "light",
    primary: { main: "#1976d2" },
    background: { default: "#f5f5f5", paper: "#ffffff" },
    text: { primary: "#333333", secondary: "#555555" }
  },
});

//<Header />
//  Sidebar 
const Sidebar = ({ children }) => (
    <Box sx={{ width: 280, height: '100vh', background: "#e8f4f8", p: 3, color: '#333333' }}>
    {children}
  </Box>
);
Sidebar.propTypes = { children: PropTypes.node.isRequired };

const sidebarItems = [
  { icon: <WorkIcon />, text: 'Job Listings' },
  { icon: <Notifications />, text: 'Notifications' },
  { icon: <Settings />, text: 'Preferences' },
];

// Job Card
const VisaJobCard = ({ job }) => (
  <Card sx={{
    background: job.isSponsor ? "#f9f9f9" : "#fdecea",
    borderRadius: 2,
    border: `1px solid ${ job.isSponsor ? "#dddddd" : "#f44336" } `,
    boxShadow: 1,
    mb: 2
  }}>
    <CardContent>
      <Typography variant="h6" sx={{ mb: 1, color: '#1976d2' }}>
        {job.title}
      </Typography>
      <Typography variant="body2" sx={{ color: '#555555' }}>
        {job.organization}
      </Typography>
      <Typography variant="body2" sx={{ mt: 1, color: '#777777' }}>
        {job.locationsDerived?.join(', ')} • {new Date(job.datePosted).toLocaleDateString()}
      </Typography>
      <Divider sx={{ my: 1, borderColor: '#eeeeee' }} />
      <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
        <Chip
          label={job.isSponsor ? "Sponsored" : "Not Sponsored"}
          size="small"
          sx={{
            bgcolor: job.isSponsor ? 'success.light' : 'error.light',
            color: job.isSponsor ? 'success.contrastText' : 'error.contrastText'
          }}
        />
        <Chip
          label="Apply"
          component="a"
          href={job.url}
          clickable
          size="small"
          variant="outlined"
        />
      </Box>
    </CardContent>
  </Card>
);
VisaJobCard.propTypes = {
  job: PropTypes.shape({
    title: PropTypes.string,
    organization: PropTypes.string,
    url: PropTypes.string,
    datePosted: PropTypes.string,
    locationsDerived: PropTypes.arrayOf(PropTypes.string),
    isSponsor: PropTypes.bool
  }).isRequired
};

// Main Dashboard 
export default function VisaJobDashboard() {
  const [jobs, setJobs] = useState([]);
  const [filter, setFilter] = useState("");
  const [page, setPage] = useState(1);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState(null);

  const ITEMS_PER_PAGE = 6;

  useEffect(() => { fetchJobs() }, []);

  async function fetchJobs() {
    setIsLoading(true);
    setError(null);
    try {
      const res = await fetch('/api/jobs?limit=100&includeNonSponsors=true');
      if (!res.ok) throw new Error(`HTTP ${ res.status } `);
      const data = await res.json();
      setJobs(data);
    } catch (e) {
      setError(e.message);
    } finally {
      setIsLoading(false);
    }
  }

  // filter
  const filtered = jobs.filter(j =>
    j.title.toLowerCase().includes(filter.toLowerCase()) ||
    j.organization.toLowerCase().includes(filter.toLowerCase()) ||
    j.locationsDerived?.join(', ').toLowerCase().includes(filter.toLowerCase())
  );

  // pagination
  const pageCount = Math.ceil(filtered.length / ITEMS_PER_PAGE);
  const paged = filtered.slice((page - 1) * ITEMS_PER_PAGE, page * ITEMS_PER_PAGE);

  return (
    <ThemeProvider theme={lightTheme}>
      <Box sx={{ display: 'flex' }}>
        <Sidebar>
          <Box sx={{ display: 'flex', alignItems: 'center', mb: 4 }}>
            <Avatar sx={{ bgcolor: '#1976d2', mr: 2 }}>T</Avatar>
            <Typography variant="h6">Test</Typography>
          </Box>
          <List>
            {sidebarItems.map(item => (
              <ListItem key={item.text} disablePadding sx={{ mb: 1 }}>
                <ListItemIcon>{item.icon}</ListItemIcon>
                <ListItemText primary={item.text} />
              </ListItem>
            ))}
          </List>
        </Sidebar>
        <Box sx={{ flexGrow: 1, p: 4, background: '#fff' }}>
          <Typography variant="h4" sx={{ mb: 3 }}>Data Engineer Openings</Typography>

          <TextField
            fullWidth
            placeholder="Filter by title, company or location"
            value={filter}
            onChange={e => { setFilter(e.target.value); setPage(1) }}
            InputProps={{
              startAdornment: <InputAdornment position="start"><SearchIcon /></InputAdornment>,
              sx: { mb: 3, borderRadius: 2 }
            }}
          />

          {error && <Alert severity="error">{error}</Alert>}
          {isLoading ? (
            <Box sx={{ display: 'flex', justifyContent: 'center', mt: 4 }}>
              <CircularProgress />
            </Box>
          ) : filtered.length === 0 ? (
            <Alert severity="info">No jobs found.</Alert>
          ) : (
            <>
              <Grid container spacing={2}>
                {paged.map(job => (
                  <Grid item xs={12} md={6} key={job.jobPostingId}>
                    <VisaJobCard job={job} />
                  </Grid>
                ))}
              </Grid>

              <Box sx={{ display: 'flex', justifyContent: 'center', mt: 2 }}>
                <Pagination
                  count={pageCount}
                  page={page}
                  onChange={(_, v) => setPage(v)}
                  color="primary"
                />
              </Box>
            </>
          )}
        </Box>
      </Box>
    </ThemeProvider>
  );
}
