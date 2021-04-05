using System;
using System.Collections.Generic;
using MongoDB.Driver;

namespace MongoDB.MongoContext
{
    internal readonly struct SaveDocumentChangesContext<TDocument>
        where TDocument : IMongoAggregate<TDocument>
    {
        private readonly TrackedDocument<TDocument> _trackedDocument;
        private readonly PrimaryKeyFilterSelector<TDocument> _primaryKeyFilterSelector;

        public SaveDocumentChangesContext(TrackedDocument<TDocument> trackedDocument, PrimaryKeyFilterSelector<TDocument> primaryKeyFilterSelector)
        {
            _trackedDocument = trackedDocument;
            _primaryKeyFilterSelector = primaryKeyFilterSelector;
        }

        private FilterDefinition<TDocument> PrimaryKeyFilter => 
            _primaryKeyFilterSelector(Builders<TDocument>.Filter, _trackedDocument.Document);

        public void OnSaveChanges(SaveDocumentsChangesContext<TDocument> context)
        {
            var state = _trackedDocument.State;
            var document = _trackedDocument.Document;
            
            var mutations = document.DequeueMutations();
            context.AddMutations(mutations);

            switch (state)
            {
                case DocumentState.Added:
                    OnAdded(context, document);
                    break;
                case DocumentState.Deleted:
                    OnDeleted(context);
                    break;
                case DocumentState.Unchanged:
                {
                    if (mutations.Count > 0)
                    {
                        _trackedDocument.State = DocumentState.Modified;
                    }
                    
                    OnModified(context, mutations);
                    break;
                }
                case DocumentState.Modified:
                    OnModified(context, mutations);
                    break;
                default:
                    throw new InvalidOperationException($"The state was unknown '{state}'.");
            }
        }

        private void OnAdded(SaveDocumentsChangesContext<TDocument> context, TDocument document)
        {
            var writeModel = new InsertOneModel<TDocument>(document);
            context.AddWriteModel(writeModel);
        }

        private void OnDeleted(SaveDocumentsChangesContext<TDocument> context)
        {
            var filter = PrimaryKeyFilter;
            var writeModel = new DeleteOneModel<TDocument>(filter);
            context.AddWriteModel(writeModel);
        }

        private void OnModified(
            SaveDocumentsChangesContext<TDocument> context,
            IReadOnlyCollection<IMutation<TDocument>> mutations)
        {
            if (mutations.Count == 0)
            {
                return;
            }

            var filter = PrimaryKeyFilter;
            foreach (var mutation in mutations)
            {
                var update = mutation.ToUpdateDefinition(Builders<TDocument>.Update);
                
                var writeModel = new UpdateOneModel<TDocument>(filter, update);
                context.AddWriteModel(writeModel);
            }
        }
    }
}