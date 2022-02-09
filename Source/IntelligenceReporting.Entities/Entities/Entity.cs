using System.ComponentModel.DataAnnotations;

namespace IntelligenceReporting.Entities
{
    /// <summary>An entity</summary>
    public abstract class Entity
    {
        /// <summary>Our database id</summary>
        [Key]
        public int Id { get; set; }
    }


    /// <summary>An entity that is synchronized from a source database</summary>
    public abstract class SynchronizedEntity : Entity
    {
        /// <summary>The source of this entity</summary>
        public SourceId SourceId { get; set; }

        /// <summary>The id in the source database</summary>
        public int ExternalId { get; set; }
    }
}
